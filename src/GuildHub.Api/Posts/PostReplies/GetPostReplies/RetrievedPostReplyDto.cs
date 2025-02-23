namespace GuildHub.Api.Posts.PostReplies.GetPostReplies;

public sealed record RetrievedPostReplyByIdDto(
    Guid Id,
    string Content,
    string? ImagePath,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc)
    : IResponse;
