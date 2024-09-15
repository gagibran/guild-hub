namespace GuildHub.Api.Posts.CreatePost;

public sealed class CreatePostHandler(IApplicationDbContext applicationDbContext, IMapDispatcher mapDispatcher) : IRequestHandler<CreatePostDto, CreatedPostDto>
{
    private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public async Task<Result<CreatedPostDto>> HandleAsync(CreatePostDto createPostDto, CancellationToken cancellationToken)
    {
        Result<Title> postTitleResult = Title.Build(createPostDto.Title);
        if (!postTitleResult.IsSuccess)
        {
            return Result.Fail<CreatedPostDto>(postTitleResult);
        }
        var post = new Post(postTitleResult.Value!, createPostDto.Content, createPostDto.ImagePath);
        _applicationDbContext.Posts.Add(post);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        CreatedPostDto createdPostDto = _mapDispatcher.DispatchMapAsync<Post, CreatedPostDto>(post);
        return Result.Success(createdPostDto);
    }
}
