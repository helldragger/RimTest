using System;
using System.Runtime.Serialization;

namespace RimTest
{
    [Serializable]
    internal class InvalidTestException : Exception
    {
        public InvalidTestException()
        {
        }

        public InvalidTestException(string message) : base(message)
        {
        }

        public InvalidTestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidTestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}