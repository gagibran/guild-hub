namespace GuildHub.Api.Posts.CreatePost;

public sealed class CreatePostHandler(ApplicationDbContext applicationDbContext, IMapDispatcher mapDispatcher) : IRequestHandler<CreatePostRequest, CreatedPostDto>
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public async Task<Result<CreatedPostDto>> HandleAsync(CreatePostRequest createPostRequest, CancellationToken cancellationToken)
    {
        Result<Post> postResult = Post.Build(createPostRequest.Title, createPostRequest.Content, createPostRequest.ImagePath);
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
