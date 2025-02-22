namespace GuildHub.Api.Posts.CreatePost;

public sealed class PostToCreatedPostDtoMapper : IMapHandler<Post, CreatedPostDto>
{
    public CreatedPostDto Map(Post post)
    {
        return new(post.Id, post.Title.ToString(), post.Content?.ToString(), post.ImagePath);
    }
}
