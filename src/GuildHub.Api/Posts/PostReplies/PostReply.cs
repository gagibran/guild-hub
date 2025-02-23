namespace GuildHub.Api.Posts.PostReplies;

public sealed class PostReply : Entity
{
    public Post Post { get; }
    public Content Content { get; }
    public string? ImagePath { get; }
    public NpgsqlTsVector SearchTsVector { get; }

    private PostReply(Post post, Content content, string? imagePath)
    {
        Post = post;
        Content = content;
        ImagePath = imagePath;
        SearchTsVector = null!;
    }

    private PostReply()
    {
        Post = null!;
        Content = null!;
        SearchTsVector = null!;
    }

    public static Result<PostReply> Build(Post post, string content, string? imagePath)
    {
        Result<Content> contentResult = Content.Build(content);
        if (!contentResult.IsSuccess)
        {
            return Result<PostReply>.SetTypeToFailedResult(contentResult);
        }
        var postReply = new PostReply(post, contentResult.Value!, imagePath);
        post.PostReplies.Add(postReply);
        return Result<PostReply>.Succeed(postReply);
    }
}
