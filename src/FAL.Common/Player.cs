using System.Collections.Generic;
using System.Threading.Tasks;

namespace FAL
{
    public abstract class Player
    {
        public string Name { get; set; }

        public int Score { get; set; }

        public IList<int> Cards { get; set; }

        public abstract Task<Operation<UserOperationType>> ClientOperation();

        public abstract Task ServerOperation(Operation<ServerOperationType> operation);

        public Task ServerOperation(ServerOperationType type, params object[] arguments) => ServerOperation(Operation.Create(type, arguments));

    }
}