using System.Runtime.Serialization;

namespace CacheAPI.Services
{
    [Serializable]
    internal class OutOfCacheSizeLimitException : Exception
    {
        public OutOfCacheSizeLimitException()
        {
        }

        public OutOfCacheSizeLimitException(string? message) : base(message)
        {
        }

        public OutOfCacheSizeLimitException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OutOfCacheSizeLimitException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}