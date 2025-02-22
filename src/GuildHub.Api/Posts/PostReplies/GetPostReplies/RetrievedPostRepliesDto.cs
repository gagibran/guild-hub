namespace GuildHub.Api.Posts.PostReplies.GetPostReplies;

public sealed record RetrievedPostRepliesDto(
    List<RetrievedPostReplyDto> PostReplies,
    int? CurrentPageIndex,
    int? RepliesPerPage,
    int RepliesCount,
    int PagesCount)
    : PaginationDto(CurrentPageIndex, RepliesPerPage, RepliesCount, PagesCount),
    IResponse;
