namespace GuildHub.Api.Posts.GetPostById;

public sealed class GetPostByIdHandler(ApplicationDbContext applicationDbContext, IMapDispatcher mapDispatcher) : IRequestHandler<GetPostByIdRequest, RetrievedPostByIdDto>
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public async Task<Result<RetrievedPostByIdDto>> HandleAsync(GetPostByIdRequest getPostByIdRequest, CancellationToken cancellationToken)
    {
        Post? retrievedPost = await _applicationDbContext.Posts
            .AsNoTracking()
            .SingleOrDefaultAsync(post => post.Id == getPostByIdRequest.Id, cancellationToken);
        if (retrievedPost is null)
        {
            return Result<RetrievedPostByIdDto>.Fail($"No post with the ID '{getPostByIdRequest.Id}' was found.");
        }
        RetrievedPostByIdDto retrievedPostByIdDto = _mapDispatcher.DispatchMap<Post, RetrievedPostByIdDto>(retrievedPost);
        return Result<RetrievedPostByIdDto>.Succeed(retrievedPostByIdDto);
    }
}
