using System.Diagnostics;
using System.Net;
using System.Text.Json;
using GuildHub.Common.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GuildHub.Common;

public static class ApiHelper
{
    private static readonly JsonSerializerOptions s_jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

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

    public static void CreatePaginationHeader(HttpContext httpContext, PaginationDto paginationDto)
    {
        string paginationHeader = JsonSerializer.Serialize(paginationDto, s_jsonSerializerOptions);
        httpContext.Response.Headers.Append("X-Pagination", paginationHeader);
    }
}
