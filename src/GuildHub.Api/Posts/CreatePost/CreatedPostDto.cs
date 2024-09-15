namespace GuildHub.Api.Posts.CreatePost;

public sealed record CreatedPostDto(Guid Id, string Title, string? Content, string? ImagePath) : IResponse;
