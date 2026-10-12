using Microsoft.AspNetCore.Diagnostics;
namespace GlobalExceptionHandler
{
    public class GlobalExceptionHandlerEx : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandlerEx> logger;
        public GlobalExceptionHandlerEx(ILogger<GlobalExceptionHandlerEx> logger)
        {
            this.logger = logger;
        }
        public ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var exceptionMessage = exception.Message;
            logger.LogError(
                "Error Message: {exceptionMessage}, Time of occurrence {time}",
                exceptionMessage, DateTime.UtcNow);
            // Return false to continue with the default behavior
            // - or - return true to signal that this exception is handled
            return ValueTask.FromResult(false);
        }
    }
}
