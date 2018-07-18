using System.Collections.Generic;

namespace FAL
{
    public class Operation
    {
        public static Operation<TOperationType> Create<TOperationType>(TOperationType type, params object[] arguments) => new Operation<TOperationType>(type, arguments);
    }

    public class Operation<TOperationType>
    {
        public TOperationType Type { get; }

        public IEnumerable<object> Arguments { get; }

        public Operation(TOperationType type, IEnumerable<object> arguments)
        {
            Type = type;
            Arguments = arguments;
        }
    }
}