namespace GuildHub.Api.Posts.GetPosts;

public sealed record RetrievedPostsDto(List<RetrievedPostByIdDto> RetrievedPostByIdDtos) : IResponse;
