using GuildHub.Api.Posts.PostReplies;

namespace GuildHub.Api.Posts.GetPostById;

public sealed class PostRepliesToRetrievedPostRepliesForPostDtoMapper : IMapHandler<ICollection<PostReply>, List<RetrievedPostReplyForPostDto>>
{
    public List<RetrievedPostReplyForPostDto> Map(ICollection<PostReply> postReplies)
    {
        var retrievedPostRepliesForPostDto = new List<RetrievedPostReplyForPostDto>();
        foreach (PostReply postReply in postReplies)
        {
            retrievedPostRepliesForPostDto.Add(new RetrievedPostReplyForPostDto(
                postReply.Message,
                postReply.ImagePath,
                postReply.CreatedAtUtc));
        }
        return retrievedPostRepliesForPostDto;
    }
}
