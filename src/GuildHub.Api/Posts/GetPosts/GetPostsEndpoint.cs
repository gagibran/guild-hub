namespace GuildHub.Api.Posts.GetPosts;

public sealed class GetPostsEndpoint
{
    public static async Task<Ok<List<RetrievedPostByIdDto>>> GetPostsAsync(IRequestDispatcher dispatcher, CancellationToken cancellationToken = default)
    {
        Result<RetrievedPostsDto> retrievedPostsDtoResult = await dispatcher.DispatchRequestAsync<RetrievedPostsDto>(cancellationToken);
        return TypedResults.Ok(retrievedPostsDtoResult.Value!.RetrievedPostByIdDtos);
    }
}
