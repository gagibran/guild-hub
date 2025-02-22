namespace GuildHub.Api.Posts.PostReplies.CreatePostReply;

public sealed record CreatedPostReplyDto(Guid Id, string Content, string? ImagePath) : IResponse;
