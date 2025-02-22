namespace GuildHub.Api.Posts.PostReplies.CreatePostReply;

public static class CreatePostReplyEndpoint
{
    public static async Task<Results<ProblemHttpResult, Created<CreatedPostReplyDto>>> CreatePostReplyAsync(
        IRequestDispatcher dispatcher,
        HttpContext httpContext,
        Guid postId,
        CreatePostReplyDto createPostReplyDto,
        CancellationToken cancellationToken = default)
    {
        var createPostReplyRequest = new CreatePostReplyRequest(postId, createPostReplyDto.Content, createPostReplyDto.ImagePath);
        Result<CreatedPostReplyDto> createdPostReplyDtoResult = await dispatcher.DispatchRequestAsync<CreatePostReplyRequest, CreatedPostReplyDto>(
            createPostReplyRequest,
            cancellationToken);
        if (createdPostReplyDtoResult.IsSuccess)
        {
            return TypedResults.Created((string?)null, createdPostReplyDtoResult.Value);
        }
        if (createdPostReplyDtoResult.Errors.Contains($"No post with the ID '{postId}' was found."))
        {
            return ApiHelper.CreateProblemDetails(HttpStatusCode.NotFound, createdPostReplyDtoResult.Errors, httpContext);
        }
        return ApiHelper.CreateProblemDetails(HttpStatusCode.UnprocessableEntity, createdPostReplyDtoResult.Errors, httpContext);
    }
}
