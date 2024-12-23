namespace GuildHub.Api.Posts.DeletePostById;

public sealed class DeletePostByIdEndpoint
{
    public static async Task<Results<ProblemHttpResult, NoContent>> DeletePostByIdAsync(
        IRequestDispatcher dispatcher,
        HttpContext httpContext,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        Result retrievedPostDtoResult = await dispatcher.DispatchRequestAsync(new DeletePostByIdDto(id), cancellationToken);
        if (!retrievedPostDtoResult.IsSuccess)
        {
            return ApiHelper.CreateProblemDetails(HttpStatusCode.NotFound, retrievedPostDtoResult.Errors, httpContext);
        }
        return TypedResults.NoContent();
    }
}
