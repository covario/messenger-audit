using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Covario.AuditAdminApp.Pipeline
{
    /// <summary>
    /// Use X Forwarded Proto header to restore request scheme
    /// Required to prevent error when calling out to Auth server
    /// </summary>
    public class HttpsSaver
    {
        private readonly RequestDelegate _next;

        public HttpsSaver(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var xProto = httpContext.Request.Headers["X-Forwarded-Proto"].ToString();
            if (xProto != null && xProto.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                httpContext.Request.Scheme = "https";
            }

            return _next(httpContext);
        }
    }
}
