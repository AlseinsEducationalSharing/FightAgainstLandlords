using System;
using System.Threading.Tasks;
using FAL.Game;
using FAL.Server.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FAL.Server
{
    public class ClientPlayer : Player
    {
        private Func<IHubContext<FALHub>> _hub;

        private string _connectionId;

        private Resulter<Operation<UserOperationType>> _userOperationResulter;

        public ClientPlayer(Func<IHubContext<FALHub>> hub, string connectionId)
        {
            _hub = hub;
            _connectionId = connectionId;
            _userOperationResulter = new Resulter<Operation<UserOperationType>>();
        }

        public Task UserOperation(Operation<UserOperationType> operation) => _userOperationResulter.Result(operation);

        public override async Task<Operation<UserOperationType>> ClientOperation() => await _userOperationResulter;

        public override Task ServerOperation(Operation<ServerOperationType> operation) => _hub().Clients.Client(_connectionId).SendAsync("GameOperation", operation);
    }
}