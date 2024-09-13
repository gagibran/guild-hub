using GuildHub.Api.Common;
using GuildHub.Api.Common.ResultPattern;
using GuildHub.Api.PostReplies;

namespace GuildHub.Api.Posts;

public sealed class Post(string title, string? content, string? imagePath) : Entity
{
    public string Title { get; } = title;
    public string? Content { get; } = content;
    public string? ImagePath { get; } = imagePath;
    public ICollection<PostReply> PostReplies { get; } = [];

    public Result AddPostReply(PostReply postReply)
    {
        if (PostReplies.Any(existingPostReply => existingPostReply == postReply))
        {
            return Result.Fail("postReplyAlreadyExists", $"The reply with ID {postReply.Id} has already been added to this post.");
        }
        PostReplies.Add(postReply);
        return Result.Success();
    }
}
