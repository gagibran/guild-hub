namespace GuildHub.Api.Posts.PostReplies.CreatePostReply;

public sealed record CreatePostReplyRequest(Guid PostId, string Content, string? ImagePath) : IRequest;
