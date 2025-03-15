namespace GuildHub.Api.Posts.PostReplies;

public sealed class PostReply : Entity
{
    public Post Post { get; }
    public Content Content { get; private set; }
    public string? ImagePath { get; private set; }
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

    public Result Update(string? content, string? imagePath)
    {
        if (content is null && imagePath is null)
        {
            return Result.Fail($"At least one of the following must be provided: {nameof(content)}, or {nameof(imagePath)}.");
        }
        if (content is not null && content.Trim() == string.Empty)
        {
            return Result.Fail($"{nameof(content)} must not be empty.");
        }
        Result<Content?> contentResult = Content.BuildNullable(content);
        if (!contentResult.IsSuccess)
        {
            return contentResult;
        }
        Content = contentResult.Value ?? Content;
        ImagePath = imagePath ?? ImagePath;
        UpdatedAtUtc = DateTime.UtcNow;
        return Result.Succeed();
    }
}
