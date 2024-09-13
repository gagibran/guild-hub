using GuildHub.Api.Common.RequestHandler;

namespace GuildHub.Api.Posts.CreatePost;

public sealed record PostCreatedDto(string Title, string? Content, string? ImagePath) : IResponse;
