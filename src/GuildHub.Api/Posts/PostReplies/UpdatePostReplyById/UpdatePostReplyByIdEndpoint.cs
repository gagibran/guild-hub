namespace GuildHub.Api.Posts.PostReplies.UpdatePostReplyById;

public sealed class UpdatePostReplyByIdEndpoint
{
    public static async Task<Results<ProblemHttpResult, NoContent>> UpdatePostReplyByIdAsync(
        IRequestDispatcher dispatcher,
        HttpContext httpContext,
        Guid postId,
        Guid id,
        UpdatePostReplyByIdDto updatePostByIdDto,
        CancellationToken cancellationToken = default)
    {
        Result updatePostByIdResult = await dispatcher.DispatchRequestAsync(
            new UpdatePostReplyByIdRequest(postId, id, updatePostByIdDto),
            cancellationToken);
        if (updatePostByIdResult.IsSuccess)
        {
            return TypedResults.NoContent();
        }
        if (updatePostByIdResult.Errors.Contains($"No post with the ID '{postId}' was found.")
            || updatePostByIdResult.Errors.Contains($"No post reply with the ID '{id}' was found."))
        {
            return ApiHelper.CreateProblemDetails(HttpStatusCode.NotFound, updatePostByIdResult.Errors, httpContext);
        }
        return ApiHelper.CreateProblemDetails(HttpStatusCode.UnprocessableEntity, updatePostByIdResult.Errors, httpContext);
    }
}
