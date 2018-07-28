using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alsein.Utilities.LifetimeAnnotations;
using Autofac;
using FAL.Game;
using FAL.Server.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FAL.Server.Services
{
    [Singleton]
    public class FALServerService
    {
        public IContainer Container { get; set; }

        private IDictionary<int, IList<string>> Rooms { get; } = new Dictionary<int, IList<string>>();

        private IDictionary<string, UserInfo> Users { get; } = new Dictionary<string, UserInfo>();

        private IDictionary<int, FALGame> Games = new Dictionary<int, FALGame>();


        public (int, int) Hello() => (Users.Count, Rooms.Count);

        public bool Login(string connectionId, string userName)
        {
            if (Users.Any(x => x.Value.Name == userName))
            {
                return false;
            }
            Users.Add(connectionId, new UserInfo
            {
                Name = userName,
                ConnectionId = connectionId,
                Score = 0
            });
            return true;
        }

        public bool Join(string connectionId, int room, FALHub hub)
        {
            if (!Rooms.ContainsKey(room))
            {
                Rooms.Add(room, new List<string>());
            }
            if (Rooms[room].Contains(connectionId))
            {
                return true;
            }
            if (Rooms[room].Count == 3)
            {
                return false;
            }
            Rooms[room].Add(connectionId);

            if (Rooms[room].Count == 3)
            {
                var players = new List<ClientPlayer>();

                foreach (var client in Rooms[room])
                {
                    var info = Users[client];
                    players.Add(info.CurrentPlayer = new ClientPlayer(Container.Resolve<IHubContext<FALHub>>, client)
                    {
                        Name = info.Name,
                        Score = info.Score
                    });
                }
                var game = new FALGame(players.ToArray());
                _ = Task.Run(game.Play);
            }
            return true;
        }

        public Task GameOperation(string connectionId, Operation<UserOperationType> operation) => Users[connectionId].CurrentPlayer.SendToDownstreamAsync(operation);

        public void Disconnect(string connectionId)
        {
            Users.Remove(connectionId);
        }

    }
}