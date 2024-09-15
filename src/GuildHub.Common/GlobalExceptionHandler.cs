using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GuildHub.Common;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        string traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        logger.LogError(
            "An exception of type '{exceptionType}' was thrown. Error message: {errorMessage} Trace ID: {traceId}.",
            exception.GetType(),
            exception.Message,
            traceId);
        await Results
            .Problem(
                title: "An unexpected error occurred. Please, contact support and give them the trace ID to further assist you.",
                extensions: new Dictionary<string, object?> { { "traceId", traceId } })
            .ExecuteAsync(httpContext);
        return true;
    }
}
