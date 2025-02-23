namespace GuildHub.Api.Posts.PostReplies.GetPostReplies;

public sealed class PagedPostRepliesToRetrievedPostRepliesDtoMapper : IMapHandler<PagedList<PostReply>, RetrievedPostRepliesDto>
{
    public RetrievedPostRepliesDto Map(PagedList<PostReply> pagedPostReplies)
    {
        return new RetrievedPostRepliesDto(
            [
                .. pagedPostReplies.EntitiesInPage.Select(postReply => new RetrievedPostReplyByIdDto(
                    postReply.Id,
                    postReply.Content.ToString(),
                    postReply.ImagePath,
                    postReply.CreatedAtUtc,
                    postReply.UpdatedAtUtc))
            ],
            pagedPostReplies.CurrentPageIndex,
            pagedPostReplies.EntitiesPerPage,
            pagedPostReplies.EntitiesCount,
            pagedPostReplies.PagesCount);
    }
}
