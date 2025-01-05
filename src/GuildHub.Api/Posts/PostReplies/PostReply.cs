namespace GuildHub.Api.Posts.PostReplies;

public sealed class PostReply : Entity
{
    public Post Post { get; }
    public Content Content { get; }
    public string? ImagePath { get; }

    private PostReply(Post post, Content content, string? imagePath)
    {
        Post = post;
        Content = content;
        ImagePath = imagePath;
    }

    private PostReply()
    {
        Post = null!;
        Content = null!;
    }

    public static Result<PostReply> Build(Post post, string content, string? imagePath)
    {
        Result<Content?> contentResult = Content.Build(content);
        if (!contentResult.IsSuccess)
        {
            return Result<PostReply>.SetTypeToFailedResult(contentResult);
        }
        return Result<PostReply>.Succeed(new PostReply(post, contentResult.Value!, imagePath));
    }
}
