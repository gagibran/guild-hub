namespace GuildHub.Api.Posts.GetPosts;

public sealed class GetPostsEndpoint
{
    public static async Task<Results<ProblemHttpResult, Ok<List<RetrievedPostByIdDto>>>> GetPostsAsync(
        string? query,
        int? currentPageIndex,
        int? postsPerPage,
        IRequestDispatcher dispatcher,
        HttpContext httpContext,
        string sortBy = "None",
        CancellationToken cancellationToken = default)
    {
        var getPostsDto = new GetPostsDto(query, currentPageIndex, postsPerPage, sortBy);
        Result<RetrievedPostsDto> retrievedPostsDtoResult = await dispatcher.DispatchRequestAsync<GetPostsDto, RetrievedPostsDto>(
            getPostsDto,
            cancellationToken);
        if (!retrievedPostsDtoResult.IsSuccess)
        {
            return ApiHelper.CreateProblemDetails(HttpStatusCode.UnprocessableEntity, retrievedPostsDtoResult.Errors, httpContext);
        }
        ApiHelper.CreatePaginationHeader(httpContext, retrievedPostsDtoResult.Value!);
        return TypedResults.Ok(retrievedPostsDtoResult.Value!.Posts);
    }
}
