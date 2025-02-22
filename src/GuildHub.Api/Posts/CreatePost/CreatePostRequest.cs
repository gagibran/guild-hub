namespace GuildHub.Api.Posts.CreatePost;

public sealed record CreatePostRequest(string Title, string? Content, string? ImagePath) : IRequest;
