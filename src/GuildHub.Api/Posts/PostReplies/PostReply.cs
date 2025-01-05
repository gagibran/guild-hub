namespace GuildHub.Api.Posts.PostReplies;

public sealed class PostReply : Entity
{
    public Post Post { get; }
    public string Message { get; }
    public string? ImagePath { get; }

    public PostReply(Post post, string message, string? imagePath)
    {
        Post = post;
        Message = message;
        ImagePath = imagePath;
    }

    private PostReply()
    {
        Post = null!;
        Message = string.Empty;
    }
}
