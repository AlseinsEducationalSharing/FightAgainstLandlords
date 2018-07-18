using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace FAL.Client
{
    public class LocalPlayer : Player
    {
        private HubConnection _hubConnection;
        private Resulter<Operation<ServerOperationType>> _serverOperationResulter;

        private Resulter<Operation<UserOperationType>> _clientOperationResulter;

        public LocalPlayer(HubConnection hubConnection)
        {
            _hubConnection = hubConnection;
            _serverOperationResulter = new Resulter<Operation<ServerOperationType>>();
            _clientOperationResulter = new Resulter<Operation<UserOperationType>>();
        }

        public override async Task<Operation<UserOperationType>> ClientOperation() => await _clientOperationResulter;

        public override Task ServerOperation(Operation<ServerOperationType> operation) => _serverOperationResulter.Result(operation);

        public Task SendOperation(Operation<UserOperationType> operation) => _clientOperationResulter.Result(operation);

        public async Task<Operation<ServerOperationType>> GetOperation() => await _serverOperationResulter;
    }
}