namespace GuildHub.Api.Posts.PostReplies.CreatePostReply;

public static class CreatePostReplyEndpoint
{
    public static async Task<Results<ProblemHttpResult, Created>> CreatePostReplyAsync(
        IRequestDispatcher dispatcher,
        HttpContext httpContext,
        Guid postId,
        CreatePostReplyDto createPostReplyDto,
        CancellationToken cancellationToken = default)
    {
        var createPostReplyRequest = new CreatePostReplyRequest(postId, createPostReplyDto.Content, createPostReplyDto.ImagePath);
        Result createPostReplyResult = await dispatcher.DispatchRequestAsync(createPostReplyRequest, cancellationToken);
        if (createPostReplyResult.IsSuccess)
        {
            return TypedResults.Created();
        }
        if (createPostReplyResult.Errors.Contains($"No post with the ID '{postId}' was found."))
        {
            return ApiHelper.CreateProblemDetails(HttpStatusCode.NotFound, createPostReplyResult.Errors, httpContext);
        }
        return ApiHelper.CreateProblemDetails(HttpStatusCode.UnprocessableEntity, createPostReplyResult.Errors, httpContext);
    }
}
