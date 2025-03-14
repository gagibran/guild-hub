namespace GuildHub.Api.Posts;

public sealed class Post : Entity
{
    private readonly ICollection<PostReply> _postReplies;

    public Title Title { get; private set; }
    public Content? Content { get; private set; }
    public string? ImagePath { get; private set; }
    public ICollection<PostReply> PostReplies { get { return _postReplies; } }
    public NpgsqlTsVector SearchTsVector { get; }

    private Post(Title title, Content? content, string? imagePath)
    {
        Title = title;
        Content = content;
        ImagePath = imagePath;
        SearchTsVector = null!;
        _postReplies = [];
    }

    private Post()
    {
        Title = null!;
        Content = null;
        ImagePath = null;
        SearchTsVector = null!;
        _postReplies = [];
    }

    public static Result<Post> Build(string title, string? content, string? imagePath)
    {
        Result<Title> titleResult = Title.Build(title);
        Result<Content?> contentResult = Content.BuildNullable(content);
        Result combinedResults = Result.Combine(contentResult, titleResult);
        if (!combinedResults.IsSuccess)
        {
            return Result<Post>.SetTypeToFailedResult(combinedResults);
        }
        return Result<Post>.Succeed(new Post(titleResult.Value!, contentResult.Value, imagePath));
    }

    public Result Update(string? title, string? content, string? imagePath)
    {
        if (title is null && content is null && imagePath is null)
        {
            return Result.Fail($"At least one of the following must be provided: {nameof(title)}, {nameof(content)}, or {nameof(imagePath)}.");
        }
        Result<Title?> titleResult = Title.BuildNullable(title);
        Result<Content?> contentResult = Content.BuildNullable(content);
        Result combinedResult = Result.Combine(titleResult, contentResult);
        if (!combinedResult.IsSuccess)
        {
            return combinedResult;
        }
        Title = titleResult.Value ?? Title;
        Content = contentResult.Value ?? Content;
        ImagePath = imagePath ?? ImagePath;
        UpdatedAtUtc = DateTime.UtcNow;
        return Result.Succeed();
    }
}
