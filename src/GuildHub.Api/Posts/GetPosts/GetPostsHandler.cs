namespace GuildHub.Api.Posts.GetPosts;

public sealed class GetPostsHandler(IApplicationDbContext applicationDbContext, IMapDispatcher mapDispatcher)
    : IRequestHandler<GetPostsDto, RetrievedPostsDto>
{
    private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public async Task<Result<RetrievedPostsDto>> HandleAsync(GetPostsDto getPostsDto, CancellationToken cancellationToken)
    {
        IQueryable<Post> posts = _applicationDbContext.Posts;
        var isQueryNullOrWhitespace = string.IsNullOrWhiteSpace(getPostsDto.Query);
        if (!isQueryNullOrWhitespace)
        {
            posts = posts
                .Where(post => post.SearchTsVector.Matches(EF.Functions.PhraseToTsQuery("english", getPostsDto.Query!)))
                .Select(post => post);
        }
        if(!Enum.TryParse(getPostsDto.SortBy, true, out SortByType sortByType))
        {
            return Result.Fail<RetrievedPostsDto>(
                $"Cannot sort by '{getPostsDto.SortBy}'. The valid options are: [{string.Join(", ", Enum.GetNames<SortByType>())}].");
        }
        if (isQueryNullOrWhitespace
            && (sortByType == SortByType.Relevance || sortByType == SortByType.RelevanceAsc || sortByType == SortByType.Hot))
        {
            return Result.Fail<RetrievedPostsDto>($"Cannot sort by '{sortByType}' without a search query.");
        }
        posts = sortByType switch
        {
            SortByType.Relevance => posts.OrderByDescending(post => post.SearchTsVector.Rank(EF.Functions.PhraseToTsQuery("english", getPostsDto.Query!))),
            SortByType.RelevanceAsc => posts.OrderBy(post => post.SearchTsVector.Rank(EF.Functions.PhraseToTsQuery("english", getPostsDto.Query!))),
            SortByType.Date => posts.OrderByDescending(post => post.CreatedAtUtc),
            SortByType.DateAsc => posts.OrderBy(post => post.CreatedAtUtc),
            SortByType.Hot => posts
                .OrderByDescending(post => post.CreatedAtUtc)
                .ThenByDescending(post => post.PostReplies.Count)
                .ThenByDescending(post => post.SearchTsVector.Rank(EF.Functions.PhraseToTsQuery("english", getPostsDto.Query!))),
            _ => posts
        };
        PagedList<Post> pagedPosts = await PagedList<Post>.BuildAsync(
            posts,
            getPostsDto.CurrentPageIndex,
            getPostsDto.PostsPerPage,
            cancellationToken);
        RetrievedPostsDto retrievedPostByIdDtos = _mapDispatcher.DispatchMap<PagedList<Post>, RetrievedPostsDto>(pagedPosts);
        return Result.Success(retrievedPostByIdDtos);
    }
}
