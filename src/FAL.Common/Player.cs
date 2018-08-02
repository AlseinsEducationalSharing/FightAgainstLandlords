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

        protected readonly IAsyncDataEndPoint _upstream;

        protected readonly IAsyncDataEndPoint _downstream;

        public Player()
        {
            (_upstream, _downstream) = AsyncDataEndPoint.CreateDuplex();
        }

        public Task SendAsync(Operation<ServerOperationType> operation) => _upstream.SendAsync(operation);

        public Task SendAsync(ServerOperationType type, params object[] args) => SendAsync(Operation.Create(type, args));

        public Task<Operation<UserOperationType>> ReceiveAsync() => _upstream.ReceiveAsync<Operation<UserOperationType>>();

        public event Func<object, Task> Receive { add => _upstream.Receive += value; remove => _upstream.Receive -= value; }


    }
}