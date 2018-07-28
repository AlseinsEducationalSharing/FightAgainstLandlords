using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alsein.Utilities.IO;

namespace FAL
{
    public abstract class Player
    {
        public string Name { get; set; }

        public int Score { get; set; }

        public IList<int> Cards { get; set; }

        private readonly IAsyncDataEndPoint _upstream;

        private readonly IAsyncDataEndPoint _downstream;

        public Player()
        {
            (_upstream, _downstream) = AsyncDataEndPoint.CreateDuplex();
        }

        public Task SendToUpstreamAsync(Operation<ServerOperationType> operation) => _upstream.SendAsync(operation);

        public Task SendToUpstreamAsync(ServerOperationType type, params object[] args) => SendToUpstreamAsync(Operation.Create(type, args));

        public async Task<Operation<UserOperationType>> ReceiveFromUpstreamAsync() => (await _upstream.ReceiveAsync<Operation<UserOperationType>>()).Result;

        public event Func<object, Task> ReceiveFromUpstream { add => _upstream.Receive += value; remove => _upstream.Receive -= value; }

        public Task SendToDownstreamAsync(Operation<UserOperationType> operation) => _downstream.SendAsync(operation);

        public Task SendToUpstreamAsync(UserOperationType type, params object[] args) => SendToDownstreamAsync(Operation.Create(type, args));

        public async Task<Operation<ServerOperationType>> ReceiveFromDownstreamAsync() => (await _downstream.ReceiveAsync<Operation<ServerOperationType>>()).Result;

        public event Func<object, Task> ReceiveFromDownstream { add => _downstream.Receive += value; remove => _downstream.Receive -= value; }

    }
}