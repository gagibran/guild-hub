namespace GuildHub.Api.Posts.GetPosts;

public sealed class GetPostsHandler(ApplicationDbContext applicationDbContext, IMapDispatcher mapDispatcher)
    : IRequestHandler<GetPostsRequest, RetrievedPostsDto>
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public async Task<Result<RetrievedPostsDto>> HandleAsync(GetPostsRequest getPostsRequest, CancellationToken cancellationToken)
    {
        string sortBy = getPostsRequest.QueryParameters.SortBy ?? SortPostsByType.None.ToString();
        if(!Enum.TryParse(sortBy, true, out SortPostsByType sortPostsByType))
        {
            return Result<RetrievedPostsDto>.Fail(
                $"Cannot sort by '{sortBy}'. The valid options are: [{string.Join(", ", Enum.GetNames<SortPostsByType>())}].");
        }
        string? search = getPostsRequest.QueryParameters.Search;
        bool isSearchValid = !string.IsNullOrWhiteSpace(search);
        IQueryable<Post> posts = _applicationDbContext.Posts;
        if (isSearchValid)
        {
            posts = posts
                .Where(post => post.SearchTsVector.Matches(EF.Functions.PhraseToTsQuery("english", search!)))
                .Select(post => post)
                .AsNoTracking();
        }
        if (!isSearchValid
            && (sortPostsByType == SortPostsByType.Relevance || sortPostsByType == SortPostsByType.RelevanceAsc || sortPostsByType == SortPostsByType.Hot))
        {
            return Result<RetrievedPostsDto>.Fail($"Cannot sort by '{sortPostsByType}' without a search term.");
        }
        posts = sortPostsByType switch
        {
            SortPostsByType.Relevance => posts.OrderByDescending(post => post.SearchTsVector.Rank(EF.Functions.PhraseToTsQuery("english", search!))),
            SortPostsByType.RelevanceAsc => posts.OrderBy(post => post.SearchTsVector.Rank(EF.Functions.PhraseToTsQuery("english", search!))),
            SortPostsByType.Date => posts.OrderByDescending(post => post.CreatedAtUtc),
            SortPostsByType.DateAsc => posts.OrderBy(post => post.CreatedAtUtc),
            SortPostsByType.Hot => posts
                .OrderByDescending(post => post.CreatedAtUtc)
                .ThenByDescending(post => post.PostReplies.Count)
                .ThenByDescending(post => post.SearchTsVector.Rank(EF.Functions.PhraseToTsQuery("english", search!))),
            _ => posts
        };
        PagedList<Post> pagedPosts = await PagedList<Post>.BuildAsync(
            posts,
            getPostsRequest.QueryParameters.CurrentPageIndex,
            getPostsRequest.QueryParameters.EntitiesPerPage,
            cancellationToken);
        RetrievedPostsDto retrievedPostByIdDtos = _mapDispatcher.DispatchMap<PagedList<Post>, RetrievedPostsDto>(pagedPosts);
        return Result<RetrievedPostsDto>.Succeed(retrievedPostByIdDtos);
    }
}
