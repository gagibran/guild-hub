namespace GuildHub.Api.Posts.GetPosts;

public sealed class GetPostsEndpoint
{
    public static async Task<Results<ProblemHttpResult, Ok<List<RetrievedPostByIdDto>>>> GetPostsAsync(
        [AsParameters] QueryParameters queryParameters,
        IRequestDispatcher dispatcher,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        Result<RetrievedPostsDto> retrievedPostsDtoResult = await dispatcher.DispatchRequestAsync<GetPostsRequest, RetrievedPostsDto>(
            new GetPostsRequest(queryParameters),
            cancellationToken);
        if (!retrievedPostsDtoResult.IsSuccess)
        {
            return ApiHelper.CreateProblemDetails(HttpStatusCode.UnprocessableEntity, retrievedPostsDtoResult.Errors, httpContext);
        }
        ApiHelper.CreatePaginationHeader(httpContext, retrievedPostsDtoResult.Value!);
        return TypedResults.Ok(retrievedPostsDtoResult.Value!.Posts);
    }
}
