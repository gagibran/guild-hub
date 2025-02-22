namespace GuildHub.Api.Posts.PostReplies.GetPostReplies;

public sealed record GetPostRepliesRequest(Guid PostId, QueryParameters QueryParameters) : IRequest;