namespace GuildHub.Api.Posts.GetPostById;

public sealed record RetrievedPostByIdDto(string Title, string? Content, string? ImagePath, List<RetrievedPostReplyForPostDto> Replies)
    : IResponse;
