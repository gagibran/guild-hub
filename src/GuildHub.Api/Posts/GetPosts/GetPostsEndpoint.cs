namespace GuildHub.Api.Posts.GetPosts;

public sealed class GetPostsEndpoint
{
    public static async Task<Ok<List<RetrievedPostByIdDto>>> GetPostsAsync(
        int? currentPage,
        int? pageSize,
        HttpContext httpContext,
        IRequestDispatcher dispatcher,
        CancellationToken cancellationToken = default)
    {
        var getPostsDto = new GetPostsDto(currentPage, pageSize);
        Result<RetrievedPostsDto> retrievedPostsDtoResult = await dispatcher.DispatchRequestAsync<GetPostsDto, RetrievedPostsDto>(
            getPostsDto,
            cancellationToken);
        ApiHelper.CreatePaginationHeader(httpContext, retrievedPostsDtoResult.Value!);
        return TypedResults.Ok(retrievedPostsDtoResult.Value!.RetrievedPostByIdDtos);
    }
}
