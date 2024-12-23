namespace GuildHub.Api.Posts.GetPosts;

public sealed class GetPostsEndpoint
{
    public static async Task<Ok<List<RetrievedPostByIdDto>>> GetPostsAsync(
        int? currentPageIndex,
        int? postsPerPage,
        HttpContext httpContext,
        IRequestDispatcher dispatcher,
        CancellationToken cancellationToken = default)
    {
        var getPostsDto = new GetPostsDto(currentPageIndex, postsPerPage);
        Result<RetrievedPostsDto> retrievedPostsDtoResult = await dispatcher.DispatchRequestAsync<GetPostsDto, RetrievedPostsDto>(
            getPostsDto,
            cancellationToken);
        ApiHelper.CreatePaginationHeader(httpContext, retrievedPostsDtoResult.Value!);
        return TypedResults.Ok(retrievedPostsDtoResult.Value!.Replies);
    }
}
