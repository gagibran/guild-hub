namespace GuildHub.Api.Posts.PostReplies.DeletePostReplyById;

public sealed record DeletePostReplyByIdRequest(Guid PostId, Guid Id) : IRequest;
