using System;
using System.Threading.Tasks;
using Alsein.Utilities.IO;
using FAL.Game;
using FAL.Server.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FAL.Server
{
    public class ClientPlayer : Player
    {
        public ClientPlayer(Func<IHubContext<FALHub>> hub, string connectionId)
        {
            Receive += obj => hub().Clients.Client(connectionId).SendAsync("GameOperation", obj.Result);
        }


        public Task SendAsync(Operation<UserOperationType> operation) => _downstream.SendAsync(operation);

        public Task SendAsync(UserOperationType type, params object[] args) => SendAsync(Operation.Create(type, args));

        public new Task<Operation<ServerOperationType>> ReceiveAsync() => _downstream.ReceiveAsync<Operation<ServerOperationType>>();

        public new event Func<ReceiveEventArgs, Task> Receive { add => _downstream.Receive += value; remove => _downstream.Receive -= value; }
    }
}