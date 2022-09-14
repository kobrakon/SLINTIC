using System;
using SLINTIC.Console;

namespace SLINTIC.Exceptions
{
    [Serializable]
    public class GeneralException : Exception
    {
        public GeneralException() {}

        public GeneralException(string message): base(message)
        {

        }

        public GeneralException(string message, Exception inner): base(message, inner)
        {

        }
    }

    [Serializable]
    public class RevokeNonImposedMemberException : Exception
    {
        public RevokeNonImposedMemberException() {}

        public RevokeNonImposedMemberException(string message): base(message)
        {

        }

        public RevokeNonImposedMemberException(string message, Exception inner): base(message, inner)
        {

        }
    }

    [Serializable]
    public class MathOperationTimeoutException : Exception
    {
        public MathOperationTimeoutException() {}

        public MathOperationTimeoutException(string message): base(message)
        {

        }

        public MathOperationTimeoutException(string message, Exception inner): base(message, inner)
        {

        }
    }

    [Serializable]
    public class MathOperationException : Exception
    {
        public MathOperationException() {}

        public MathOperationException(string message): base(message)
        {

        }

        public MathOperationException(string message, Exception inner): base(message, inner)
        {

        }
    }

    [Serializable]
    public class RevokeUndefinedImposedFieldInstanceException : Exception
    {
        public RevokeUndefinedImposedFieldInstanceException() {}

        public RevokeUndefinedImposedFieldInstanceException(string message) : base(message)
        {

        }

        public RevokeUndefinedImposedFieldInstanceException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}
