namespace GuildHub.Api.Posts.PostReplies.UpdatePostReplyById;

public sealed record UpdatePostReplyByIdRequest(Guid PostId, Guid Id, UpdatePostReplyByIdDto UpdatePostReplyByIdDto) : IRequest;
