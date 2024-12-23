namespace GuildHub.Api.Posts.GetPosts;

public sealed record RetrievedPostsDto(
    List<RetrievedPostByIdDto> Replies,
    int? CurrentPageIndex,
    int? PostsPerPage,
    int PostsCount,
    int PagesCount)
    : PaginationDto(CurrentPageIndex, PostsPerPage, PostsCount, PagesCount),
    IResponse;
