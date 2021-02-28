using System;
using System.Runtime.Serialization;

namespace TicketManagement.BusinessLogic.Infrastructure
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException()
        {
        }

        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ValidationException(string message, string prop)
            : this(message)
        {
            Property = prop;
        }

        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public string Property { get; protected set; }
    }
}
