namespace GuildHub.Api.Posts.GetPostById;

public sealed record RetrievedPostByIdDto(
    Guid Id,
    string Title,
    string? Content,
    string? ImagePath,
    List<RetrievedPostReplyForPostDto> Replies,
    DateTime CreatedAt)
    : IResponse;
