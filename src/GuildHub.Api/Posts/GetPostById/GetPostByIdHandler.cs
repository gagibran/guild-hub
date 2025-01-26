namespace GuildHub.Api.Posts.GetPostById;

public sealed class GetPostByIdHandler(IApplicationDbContext applicationDbContext, IMapDispatcher mapDispatcher) : IRequestHandler<GetPostByIdDto, RetrievedPostByIdDto>
{
    private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public async Task<Result<RetrievedPostByIdDto>> HandleAsync(GetPostByIdDto getPostByIdDto, CancellationToken cancellationToken)
    {
        Post? retrievedPost = await _applicationDbContext.Posts
            .Include(post => post.PostReplies)
            .Where(post => post.Id == getPostByIdDto.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (retrievedPost is null)
        {
            return Result<RetrievedPostByIdDto>.Fail($"No post with the ID '{getPostByIdDto.Id}' was found.");
        }
        RetrievedPostByIdDto retrievedPostByIdDto = _mapDispatcher.DispatchMap<Post, RetrievedPostByIdDto>(retrievedPost);
        return Result<RetrievedPostByIdDto>.Succeed(retrievedPostByIdDto);
    }
}
