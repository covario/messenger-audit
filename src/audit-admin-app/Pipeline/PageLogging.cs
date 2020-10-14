using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Covario.AuditAdminApp.Pipeline
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class PageLogging
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PageLogging> _logger;

        public PageLogging(RequestDelegate next, ILogger<PageLogging> logger)
        {
            _next = next;
            _logger = logger;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var sb = new StringBuilder();
            foreach (var header in httpContext.Request.Headers)
            {
                sb.AppendLine($"{header.Key}:{header.Value}");
            }
            _logger.LogInformation("Request : {Scheme} {Host} {PathBase} {Path} {QueryString}",
                httpContext.Request.Scheme.ToString(),
                httpContext.Request.Host.ToString(),
                httpContext.Request.PathBase.ToString(),
                httpContext.Request.Path.ToString(),
                httpContext.Request.QueryString.ToString());
            _logger.LogInformation("Headers : {headers}", sb.ToString());
            
            return _next(httpContext);
        }
    }

    
}
