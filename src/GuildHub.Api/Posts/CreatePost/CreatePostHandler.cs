namespace GuildHub.Api.Posts.CreatePost;

public sealed class CreatePostHandler(IApplicationDbContext applicationDbContext, IMapDispatcher mapDispatcher) : IRequestHandler<CreatePostDto, CreatedPostDto>
{
    private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public async Task<Result<CreatedPostDto>> HandleAsync(CreatePostDto createPostDto, CancellationToken cancellationToken)
    {
        Result<Post> postResult = Post.Build(createPostDto.Title, createPostDto.Content, createPostDto.ImagePath);
        if (!postResult.IsSuccess)
        {
            return Result<CreatedPostDto>.SetTypeToFailedResult(postResult);
        }
        await _applicationDbContext.Posts.AddAsync(postResult.Value!, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        CreatedPostDto createdPostDto = _mapDispatcher.DispatchMap<Post, CreatedPostDto>(postResult.Value!);
        return Result<CreatedPostDto>.Succeed(createdPostDto);
    }
}
