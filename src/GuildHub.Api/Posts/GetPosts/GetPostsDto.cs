namespace GuildHub.Api.Posts.GetPosts;

public sealed record GetPostsDto(string? Query, int? CurrentPageIndex, int? PostsPerPage, string SortBy) : IRequest;
