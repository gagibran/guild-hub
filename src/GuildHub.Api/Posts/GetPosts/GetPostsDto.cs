namespace GuildHub.Api.Posts.GetPosts;

public sealed record GetPostsDto(int? CurrentPageIndex, int? PostsPerPage) : IRequest;
