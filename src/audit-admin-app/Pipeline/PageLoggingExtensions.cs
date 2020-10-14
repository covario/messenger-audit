using Microsoft.AspNetCore.Builder;

namespace Covario.AuditAdminApp.Pipeline
{
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class PageLoggingExtensions
    {
        public static IApplicationBuilder UsePageLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PageLogging>();
        }
    }
}