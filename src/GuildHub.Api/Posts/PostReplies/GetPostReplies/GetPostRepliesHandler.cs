namespace GuildHub.Api.Posts.PostReplies.GetPostReplies;

public sealed class GetPostRepliesHandler(ApplicationDbContext applicationDbContext, IMapDispatcher mapDispatcher)
    : IRequestHandler<GetPostRepliesRequest, RetrievedPostRepliesDto>
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public async Task<Result<RetrievedPostRepliesDto>> HandleAsync(GetPostRepliesRequest getPostRepliesRequest, CancellationToken cancellationToken)
    {
        Post? post = await _applicationDbContext.Posts.FindAsync(getPostRepliesRequest.PostId, cancellationToken);
        if (post is null)
        {
            return Result<RetrievedPostRepliesDto>.Fail($"Post with ID '{getPostRepliesRequest.PostId}' not found.");
        }
        string? search = getPostRepliesRequest.QueryParameters.Search;
        IQueryable<PostReply> postReplies = _applicationDbContext.PostReplies
            .Where(postReply => postReply.Post == post)
            .Select(postReply => postReply);
        if (!string.IsNullOrWhiteSpace(search))
        {
            postReplies = postReplies
                .Where(postReply => postReply.SearchTsVector.Matches(EF.Functions.PhraseToTsQuery("english", search)))
                .Select(post => post);
        }
        string sortBy = getPostRepliesRequest.QueryParameters.SortBy ?? SortPostRepliesByType.None.ToString();
        if(!Enum.TryParse(sortBy, true, out SortPostRepliesByType sortPostRepliesByType))
        {
            return Result<RetrievedPostRepliesDto>.Fail(
                $"Cannot sort by '{sortBy}'. The valid options are: [{string.Join(", ", Enum.GetNames<SortPostRepliesByType>())}].");
        }
        postReplies = sortPostRepliesByType switch
        {
            SortPostRepliesByType.Date => postReplies.OrderByDescending(post => post.CreatedAtUtc),
            SortPostRepliesByType.DateAsc => postReplies.OrderBy(post => post.CreatedAtUtc),
            _ => postReplies
        };
        PagedList<PostReply> pagedPostReplies = await PagedList<PostReply>.BuildAsync(
            postReplies,
            getPostRepliesRequest.QueryParameters.CurrentPageIndex,
            getPostRepliesRequest.QueryParameters.EntitiesPerPage,
            cancellationToken);
        RetrievedPostRepliesDto retrievedPostRepliesForPostDto = _mapDispatcher.DispatchMap<PagedList<PostReply>, RetrievedPostRepliesDto>(pagedPostReplies);
        return Result<RetrievedPostRepliesDto>.Succeed(retrievedPostRepliesForPostDto);
    }
}
