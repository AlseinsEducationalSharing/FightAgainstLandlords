using System;
using System.Threading.Tasks;
using Alsein.Utilities.IO;
using Microsoft.AspNetCore.SignalR.Client;

namespace FAL.Client
{
    public class LocalPlayer : Player
    {
        private HubConnection _hubConnection;

        public LocalPlayer(HubConnection hubConnection)
        {
            _hubConnection = hubConnection;
        }

        public Task SendAsync(Operation<UserOperationType> operation) => _downstream.SendAsync(operation);

        public Task SendAsync(UserOperationType type, params object[] args) => SendAsync(Operation.Create(type, args));

        public new Task<Operation<ServerOperationType>> ReceiveAsync() => _downstream.ReceiveAsync<Operation<ServerOperationType>>();

        public new event Func<ReceiveEventArgs, Task> Receive { add => _downstream.Receive += value; remove => _downstream.Receive -= value; }
    }
}