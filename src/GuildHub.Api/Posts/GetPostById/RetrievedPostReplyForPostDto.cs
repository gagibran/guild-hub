namespace GuildHub.Api.Posts.GetPostById;

public sealed record RetrievedPostReplyForPostDto(string Content, string? ImagePath, DateTime CreatedAt);
