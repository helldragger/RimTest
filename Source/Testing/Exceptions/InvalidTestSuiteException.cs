using System;
using System.Runtime.Serialization;

namespace RimTest
{
    [Serializable]
    internal class InvalidTestSuiteException : Exception
    {
        public InvalidTestSuiteException()
        {
        }

        public InvalidTestSuiteException(string message) : base(message)
        {
        }

        public InvalidTestSuiteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidTestSuiteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}