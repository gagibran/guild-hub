namespace GuildHub.Api.Posts;

public sealed class Post : Entity
{
    private readonly ICollection<PostReply> _postReplies;

    public Title Title { get; }
    public string? Content { get; }
    public string? ImagePath { get; }
    public ICollection<PostReply> PostReplies { get { return _postReplies; } }

    public Post(Title title, string? content, string? imagePath)
    {
        Title = title;
        Content = content;
        ImagePath = imagePath;
        _postReplies = [];
    }

    private Post()
    {
        Title = null!;
        Content = null;
        ImagePath = null;
        _postReplies = [];
    }

    public Result AddPostReply(PostReply postReply)
    {
        if (_postReplies.Any(existingPostReply => existingPostReply == postReply))
        {
            return Result.Fail($"The reply with ID {postReply.Id} has already been added to this post.");
        }
        _postReplies.Add(postReply);
        return Result.Success();
    }
}
