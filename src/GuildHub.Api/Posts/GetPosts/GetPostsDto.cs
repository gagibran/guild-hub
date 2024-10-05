namespace GuildHub.Api.Posts.GetPosts;

public sealed record GetPostsDto(int? CurrentPage, int? PageSize) : IRequest;
