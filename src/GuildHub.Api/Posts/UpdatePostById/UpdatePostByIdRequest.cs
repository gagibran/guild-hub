namespace GuildHub.Api.Posts.UpdatePostById;

public sealed record UpdatePostByIdRequest(Guid Id, string Title, string? Content, string? ImagePath) : IRequest;
