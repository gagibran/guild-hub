namespace GuildHub.Api.Posts.PostReplies.CreatePostReply;

public sealed class PostReplyToCreatedPostReplyDtoMapper : IMapHandler<PostReply, CreatedPostReplyDto>
{
    public CreatedPostReplyDto Map(PostReply postReply)
    {
        return new(postReply.Id, postReply.Content.ToString(), postReply.ImagePath);
    }
}
