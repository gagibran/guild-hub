namespace GuildHub.Api.Posts.GetPosts;

public sealed class GetPostsHandler(IApplicationDbContext applicationDbContext, IMapDispatcher mapDispatcher)
    : IRequestHandler<RetrievedPostsDto>
{
    private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public async Task<Result<RetrievedPostsDto>> HandleAsync(CancellationToken cancellationToken)
    {
        List<Post> posts = await _applicationDbContext.Posts.ToListAsync(cancellationToken);
        RetrievedPostsDto retrievedPostByIdDtos = _mapDispatcher.DispatchMap<List<Post>, RetrievedPostsDto>(posts);
        return Result.Success(retrievedPostByIdDtos);
    }
}
