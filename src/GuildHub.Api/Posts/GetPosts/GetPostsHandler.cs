namespace GuildHub.Api.Posts.GetPosts;

public sealed class GetPostsHandler(IApplicationDbContext applicationDbContext, IMapDispatcher mapDispatcher)
    : IRequestHandler<GetPostsDto, RetrievedPostsDto>
{
    private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public async Task<Result<RetrievedPostsDto>> HandleAsync(GetPostsDto getPostsDto, CancellationToken cancellationToken)
    {
        PagedList<Post> pagedPosts = await PagedList<Post>.CreatePagedListAsync(
            _applicationDbContext.Posts,
            getPostsDto.CurrentPage,
            getPostsDto.PageSize,
            cancellationToken);
        RetrievedPostsDto retrievedPostByIdDtos = _mapDispatcher.DispatchMap<PagedList<Post>, RetrievedPostsDto>(pagedPosts);
        return Result.Success(retrievedPostByIdDtos);
    }
}
