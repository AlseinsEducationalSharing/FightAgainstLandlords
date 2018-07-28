using System;
using System.Threading.Tasks;
using FAL.Game;
using FAL.Server.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FAL.Server
{
    public class ClientPlayer : Player
    {
        public ClientPlayer(Func<IHubContext<FALHub>> hub, string connectionId)
        {
            ReceiveFromDownstream += obj => hub().Clients.Client(connectionId).SendAsync("GameOperation", obj);
        }
    }
}