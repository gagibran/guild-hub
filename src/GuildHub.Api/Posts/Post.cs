namespace GuildHub.Api.Posts;

public sealed class Post : Entity
{
    public Title Title { get; }
    public string? Content { get; }
    public string? ImagePath { get; }
    public ICollection<PostReply> PostReplies { get; }

    public Post(Title title, string? content, string? imagePath)
    {
        Title = title;
        Content = content;
        ImagePath = imagePath;
        PostReplies = [];
    }

    private Post()
    {
        Title = null!;
        Content = null;
        ImagePath = null;
        PostReplies = [];
    }

    public Result AddPostReply(PostReply postReply)
    {
        if (PostReplies.Any(existingPostReply => existingPostReply == postReply))
        {
            return Result.Fail($"The reply with ID {postReply.Id} has already been added to this post.");
        }
        PostReplies.Add(postReply);
        return Result.Success();
    }
}
