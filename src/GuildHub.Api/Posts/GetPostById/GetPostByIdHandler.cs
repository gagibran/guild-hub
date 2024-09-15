namespace GuildHub.Api.Posts.GetPostById;

public sealed class GetPostByIdHandler(IApplicationDbContext applicationDbContext, IMapDispatcher mapDispatcher) : IRequestHandler<GetPostByIdDto, RetrievedPostByIdDto>
{
    private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public async Task<Result<RetrievedPostByIdDto>> HandleAsync(GetPostByIdDto getPostDto, CancellationToken cancellationToken)
    {
        Post? retrievedPost = await _applicationDbContext.Posts.FirstOrDefaultAsync(post => post.Id == getPostDto.Id, cancellationToken);
        if (retrievedPost is null)
        {
            return Result.Fail<RetrievedPostByIdDto>($"No post with the ID '{getPostDto.Id}' was found.");
        }
        RetrievedPostByIdDto retrievedPostByIdDto = _mapDispatcher.DispatchMapAsync<Post, RetrievedPostByIdDto>(retrievedPost);
        return Result.Success(retrievedPostByIdDto);
    }
}
