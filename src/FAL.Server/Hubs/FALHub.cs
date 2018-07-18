using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FAL.Game;
using FAL.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace FAL.Server.Hubs
{
    public class FALHub : Hub
    {
        public FALHub(FALServerService fALServerService)
        {
            FALServerService = fALServerService;
        }

        private FALServerService FALServerService { get; set; }

        private IDictionary<int, IList<string>> Rooms { get; } = new Dictionary<int, IList<string>>();

        private IDictionary<string, UserInfo> Users { get; } = new Dictionary<string, UserInfo>();

        private IDictionary<int, FALGame> Games = new Dictionary<int, FALGame>();

        public (int, int) Hello() => FALServerService.Hello();

        public bool Login(string userName) => FALServerService.Login(Context.ConnectionId, userName);

        public bool Join(int room) => FALServerService.Join(Context.ConnectionId, room, this);

        public Task GameOperation(Operation<UserOperationType> operation) => FALServerService.GameOperation(Context.ConnectionId, operation);

        public override Task OnDisconnectedAsync(Exception exception)
        {
            FALServerService.Disconnect(Context.ConnectionId);
            return Task.CompletedTask;
        }
    }
}