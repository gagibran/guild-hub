namespace GuildHub.Api.Posts.GetPosts;

public sealed class PagedPostsToRetrievedPostsDtoMapper(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
    : IMapHandler<PagedList<Post>, RetrievedPostsDto>
{
    private readonly LinkGenerator _linkGenerator = linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public RetrievedPostsDto Map(PagedList<Post> pagedPosts)
    {
        return new RetrievedPostsDto(
            [
                .. pagedPosts.EntitiesInPage.Select(post => new RetrievedPostByIdDto(
                    post.Id,
                    post.Title.ToString(),
                    post.Content?.ToString(),
                    post.ImagePath,
                    _linkGenerator.GetUriByName(
                        _httpContextAccessor.HttpContext!,
                        nameof(GetPostRepliesEndpoint.GetPostRepliesAsync),
                        new { postId = post.Id })!,
                    post.CreatedAtUtc,
                    post.UpdatedAtUtc))
            ],
            pagedPosts.CurrentPageIndex,
            pagedPosts.EntitiesPerPage,
            pagedPosts.EntitiesCount,
            pagedPosts.PagesCount);
    }
}
