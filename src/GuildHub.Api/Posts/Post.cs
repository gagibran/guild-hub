using NpgsqlTypes;

namespace GuildHub.Api.Posts;

public sealed class Post : Entity
{
    private readonly ICollection<PostReply> _postReplies;

    public Title Title { get; private set; }
    public Content? Content { get; private set; }
    public string? ImagePath { get; private set; }
    public ICollection<PostReply> PostReplies { get { return _postReplies; } }
    public NpgsqlTsVector SearchTsVector { get; }

    public Post(Title title, Content? content, string? imagePath)
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

    public Result UpdatePost(string title, string? content, string? imagePath)
    {
        Result<Title> titleResult = Title.Build(title);
        Result<Content?> contentResult = Content.Build(content);
        Result combinedResult = Result.Combine(titleResult, contentResult);
        if (!combinedResult.IsSuccess)
        {
            return combinedResult;
        }
        Title = titleResult.Value!;
        Content = contentResult.Value;
        ImagePath = imagePath;
        UpdatedAtUtc = DateTime.UtcNow;
        return Result.Succeed();
    }

    public Result AddPostReply(PostReply postReply)
    {
        if (_postReplies.Any(existingPostReply => existingPostReply == postReply))
        {
            return Result.Fail($"The reply with ID {postReply.Id} has already been added to this post.");
        }
        _postReplies.Add(postReply);
        return Result.Succeed();
    }
}
