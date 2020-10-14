using Microsoft.AspNetCore.Builder;

namespace Covario.AuditAdminApp.Pipeline
{
    /// <summary>
    /// Extension method used to add the HttpsSaver middleware to the HTTP request pipeline.
    /// </summary>
    public static class HttpsSaverExtensions
    {
        public static IApplicationBuilder UseHttpsSaver(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpsSaver>();
        }
    }
}