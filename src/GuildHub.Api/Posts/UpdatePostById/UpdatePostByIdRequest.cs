namespace GuildHub.Api.Posts.UpdatePostById;

public sealed record UpdatePostByIdRequest(Guid Id, UpdatePostByIdDto UpdatePostByIdDto) : IRequest;
