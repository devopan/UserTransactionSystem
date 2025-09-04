using System.Net;

namespace UserTransactionSystem.Web.Models
{
    public abstract class GenericException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        protected GenericException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            StatusCode = statusCode;
        }

        protected GenericException(string message, Exception innerException, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}