namespace GuildHub.Api.Posts.GetPostById;

public sealed record RetrievedPostByIdDto(
    Guid Id,
    string Title,
    string? Content,
    string? ImagePath,
    string Replies,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc)
    : IResponse;
