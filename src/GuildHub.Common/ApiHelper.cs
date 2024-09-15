using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GuildHub.Common;

public static class ApiHelper
{
    public static ProblemHttpResult CreateProblemDetails(HttpStatusCode httpStatusCode, List<string> errors, HttpContext httpContext)
    {
        return TypedResults.Problem(
            title: "One or more validation errors occurred.",
            statusCode: (int)httpStatusCode,
            extensions: new Dictionary<string, object?>
            {
                { "errors", errors },
                { "traceId", Activity.Current?.Id ?? httpContext.TraceIdentifier}
            });
    }
}
