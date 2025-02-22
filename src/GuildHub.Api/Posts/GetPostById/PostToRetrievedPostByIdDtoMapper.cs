namespace GuildHub.Api.Posts.GetPostById;

public sealed class PostToRetrievedPostByIdDtoMapper(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
    : IMapHandler<Post, RetrievedPostByIdDto>
{
    private readonly LinkGenerator _linkGenerator = linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public RetrievedPostByIdDto Map(Post post)
    {
        return new RetrievedPostByIdDto(
            post.Id,
            post.Title.ToString(),
            post.Content?.ToString(),
            post.ImagePath,
            _linkGenerator.GetUriByName(
                _httpContextAccessor.HttpContext!,
                nameof(GetPostRepliesEndpoint.GetPostRepliesAsync),
                new { postId = post.Id })!,
            post.CreatedAtUtc,
            post.UpdatedAtUtc);
    }
}
