namespace GuildHub.Api.Posts.GetPosts;

public sealed class PostsToRetrievedPostsDtoMapper(IMapDispatcher mapDispatcher) : IMapHandler<List<Post>, RetrievedPostsDto>
{
    public readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public RetrievedPostsDto Map(List<Post> posts)
    {
        return new RetrievedPostsDto(posts
            .Select(post => new RetrievedPostByIdDto(
                post.Title.ToString(),
                post.Content,
                post.ImagePath,
                _mapDispatcher.DispatchMap<ICollection<PostReply>, List<RetrievedPostReplyForPostDto>>(post.PostReplies)))
            .ToList());
    }
}
