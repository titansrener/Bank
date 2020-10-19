using System;
using System.Runtime.Serialization;

namespace BANCO.Core.Exceptions
{
    [Serializable]
    public class BusinessException : Exception
    {
        public BusinessException()
        {

        }
        public BusinessException(string msg) : base(msg)
        {

        }
        public BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected BusinessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}