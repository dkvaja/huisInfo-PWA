using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.JPDS.AppCore.Helpers
{
    public class HttpResponseException : Exception
    {

        public string Code { get; }

        public string ExMessage { get; }

        public int StatusCode { get; }

        public HttpResponseException(string message) : base(message) { }

        public HttpResponseException(string message, Exception innerException) : base(message, innerException) { }

        public HttpResponseException() { }

        public HttpResponseException(string exCode, string message, int statusCode) { Code = exCode; ExMessage = message; StatusCode = statusCode; }

    }
}
