using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Green.API.Middleware
{
    public class ErrorHandling
    {
        private readonly RequestDelegate _next;

        public ErrorHandling(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<Exception> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, logger);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<Exception> logger)
        {
            var code = HttpStatusCode.InternalServerError;

            if (exception is ArgumentNullException or InvalidOperationException)
                code = HttpStatusCode.BadRequest;


            if (code == HttpStatusCode.InternalServerError)
            {
                logger.LogError(exception, "Processing request exception");
            }

            var result = JsonSerializer.Serialize(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
