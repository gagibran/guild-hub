namespace GuildHub.Api.Posts.DeletePostById;

public sealed class DeletePostByIdEndpoint
{
    public static async Task<Results<ProblemHttpResult, NoContent>> DeletePostByIdAsync(
        IRequestDispatcher dispatcher,
        HttpContext httpContext,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        Result deletePostByIdResult = await dispatcher.DispatchRequestAsync(new DeletePostByIdRequest(id), cancellationToken);
        if (!deletePostByIdResult.IsSuccess)
        {
            return ApiHelper.CreateProblemDetails(HttpStatusCode.NotFound, deletePostByIdResult.Errors, httpContext);
        }
        return TypedResults.NoContent();
    }
}
