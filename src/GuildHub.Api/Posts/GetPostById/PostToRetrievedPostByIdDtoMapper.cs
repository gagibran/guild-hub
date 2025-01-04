namespace GuildHub.Api.Posts.GetPostById;

public sealed class PostToRetrievedPostByIdDtoMapper(IMapDispatcher mapDispatcher) : IMapHandler<Post, RetrievedPostByIdDto>
{
    private readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public RetrievedPostByIdDto Map(Post post)
    {
        List<RetrievedPostReplyForPostDto> retrievedPostRepliesForPostDto = _mapDispatcher.DispatchMap<ICollection<PostReply>, List<RetrievedPostReplyForPostDto>>(post.PostReplies);
        return new RetrievedPostByIdDto(
            post.Id,
            post.Title.ToString(),
            post.Content?.ToString(),
            post.ImagePath,
            retrievedPostRepliesForPostDto,
            post.CreatedAtUtc);
    }
}
