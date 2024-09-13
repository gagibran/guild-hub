using GuildHub.Api.Common.RequestHandler;

namespace GuildHub.Api.Posts.CreatePost;

public sealed record CreatePostDto(string Title, string? Content, string? ImagePath) : IRequest;
