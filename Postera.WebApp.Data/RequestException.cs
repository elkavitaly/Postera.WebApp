using System;
using System.Net;
using System.Runtime.Serialization;

namespace Postera.WebApp.Data
{
    [Serializable]
    public class RequestException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public RequestException()
        {
        }

        public RequestException(HttpStatusCode statusCode, string message) : this(message)
        {
            StatusCode = statusCode;
        }

        public RequestException(string message) : base(message)
        {
        }

        public RequestException(string message, Exception inner) : base(message, inner)
        {
        }

        protected RequestException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}