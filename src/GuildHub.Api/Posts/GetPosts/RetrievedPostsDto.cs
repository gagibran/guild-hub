namespace GuildHub.Api.Posts.GetPosts;

public sealed record RetrievedPostsDto(
    List<RetrievedPostByIdDto> RetrievedPostByIdDtos,
    int? CurrentPage,
    int? PageSize,
    int TotalAmountOfPosts,
    int TotalNumberOfPages)
    : PaginationDto(CurrentPage, PageSize, TotalAmountOfPosts, TotalNumberOfPages),
    IResponse;
