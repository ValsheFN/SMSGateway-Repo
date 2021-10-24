using SMSGateway.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SMSGateway.Client.Exceptions
{
    public class ApiException : Exception
    {
        public ApiErrorResponse ApiErrorResponse { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ApiException(ApiErrorResponse error, HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public ApiException(ApiErrorResponse error)
        {
            ApiErrorResponse = error;
        }
    }
}
