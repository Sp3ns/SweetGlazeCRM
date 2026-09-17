using System;
using System.Net;

namespace SweetGlazeCRM.winform.Services
{
    /// <summary>
    /// Thrown when the API responds but reports a failure (validation error,
    /// conflict, not found, etc). The Message is safe to display directly to the user.
    /// </summary>
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApiException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
