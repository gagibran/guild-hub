namespace GuildHub.Api.Posts.PostReplies.GetPostReplies;

public sealed class GetPostRepliesEndpoint
{
    public static async Task<Results<ProblemHttpResult, Ok<List<RetrievedPostReplyDto>>>> GetPostRepliesAsync(
        Guid postId,
        [AsParameters] QueryParameters queryParameters,
        IRequestDispatcher dispatcher,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        Result<RetrievedPostRepliesDto> retrievedPostRepliesDtoResult = await dispatcher.DispatchRequestAsync<GetPostRepliesRequest, RetrievedPostRepliesDto>(
            new GetPostRepliesRequest(postId, queryParameters),
            cancellationToken);
        if (retrievedPostRepliesDtoResult.IsSuccess)
        {
            ApiHelper.CreatePaginationHeader(httpContext, retrievedPostRepliesDtoResult.Value!);
            return TypedResults.Ok(retrievedPostRepliesDtoResult.Value!.PostReplies);
        }
        if (retrievedPostRepliesDtoResult.Errors.Contains($"Post with ID '{postId}' not found."))
        {
            return ApiHelper.CreateProblemDetails(HttpStatusCode.NotFound, retrievedPostRepliesDtoResult.Errors, httpContext);
        }
        return ApiHelper.CreateProblemDetails(HttpStatusCode.UnprocessableEntity, retrievedPostRepliesDtoResult.Errors, httpContext);
    }
}
