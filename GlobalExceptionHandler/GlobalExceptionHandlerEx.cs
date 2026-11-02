using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GlobalExceptionHandler
{
    public class GlobalExceptionHandlerEx : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandlerEx> logger;

        public GlobalExceptionHandlerEx(ILogger<GlobalExceptionHandlerEx> logger)
        {
            this.logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // 1. Naplózás
            logger.LogError(
                exception,
                "Error Message: {message}, Time of occurrence {time}",
                exception.Message, DateTime.UtcNow);

            // 2. HTTP Válasz összeállítása a kliens számára
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Váratlan szerverhiba történt.",
                Detail = exception.Message // Éles környezetben ezt elrejthetjük
            };

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            // 3. True-t adunk vissza, jelezve, hogy a hibát lekezeltük, ne fusson tovább a lánc!
            return true;
        }
    }
}