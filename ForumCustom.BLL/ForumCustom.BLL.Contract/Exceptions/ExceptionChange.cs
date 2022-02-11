using System;

namespace ForumCustom.BLL.Contract.Exceptions
{
    public class ChangeException : Exception
    {
        public ChangeException()
        {
        }

        public ChangeException(string message)
            : base(message)
        {
        }

        public ChangeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class OutdatedException : Exception
    {
        public OutdatedException() : base("You have outdated data, please update and try again")
        {
        }

        public OutdatedException(string message)
            : base(message)
        {
        }

        public OutdatedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}