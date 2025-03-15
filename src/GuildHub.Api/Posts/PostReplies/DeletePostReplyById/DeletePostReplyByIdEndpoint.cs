namespace GuildHub.Api.Posts.PostReplies.DeletePostReplyById;

public sealed class DeletePostReplyByIdEndpoint
{
    public static async Task<Results<ProblemHttpResult, NoContent>> DeletePostReplyByIdAsync(
        IRequestDispatcher dispatcher,
        HttpContext httpContext,
        Guid postId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        Result deletePostReplyByIdResult = await dispatcher.DispatchRequestAsync(new DeletePostReplyByIdRequest(postId, id), cancellationToken);
        if (!deletePostReplyByIdResult.IsSuccess)
        {
            return ApiHelper.CreateProblemDetails(HttpStatusCode.NotFound, deletePostReplyByIdResult.Errors, httpContext);
        }
        return TypedResults.NoContent();
    }
}
