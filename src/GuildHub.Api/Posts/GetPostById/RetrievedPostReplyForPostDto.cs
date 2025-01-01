namespace GuildHub.Api.Posts.GetPostById;

public sealed record RetrievedPostReplyForPostDto(string Message, string? ImagePath, DateTime CreatedAt);
