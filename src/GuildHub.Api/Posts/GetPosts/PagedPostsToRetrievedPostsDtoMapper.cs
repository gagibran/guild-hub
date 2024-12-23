using GuildHub.Common.Pagination;

namespace GuildHub.Api.Posts.GetPosts;

public sealed class PagedPostsToRetrievedPostsDtoMapper(IMapDispatcher mapDispatcher) : IMapHandler<PagedList<Post>, RetrievedPostsDto>
{
    public readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public RetrievedPostsDto Map(PagedList<Post> pagedPosts)
    {
        return new RetrievedPostsDto(
            pagedPosts.EntitiesInPage
                .Select(post => new RetrievedPostByIdDto(
                    post.Title.ToString(),
                    post.Content,
                    post.ImagePath,
                    _mapDispatcher.DispatchMap<ICollection<PostReply>, List<RetrievedPostReplyForPostDto>>(post.PostReplies)))
                .ToList(),
            pagedPosts.CurrentPageIndex,
            pagedPosts.EntitiesPerPage,
            pagedPosts.EntitiesCount,
            pagedPosts.PagesCount);
    }
}
