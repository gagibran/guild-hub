namespace GuildHub.Api.Posts.CreatePost;

public sealed class CreatePostHandler(IApplicationDbContext applicationDbContext, IMapDispatcher mapDispatcher) : IRequestHandler<CreatePostDto, CreatedPostDto>
{
    private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly IMapDispatcher _mapDispatcher = mapDispatcher;

    public async Task<Result<CreatedPostDto>> HandleAsync(CreatePostDto createPostDto, CancellationToken cancellationToken)
    {
        Result<Title> titleResult = Title.Build(createPostDto.Title);
        Result<Content?> contentResult = Content.Build(createPostDto.Content);
        Result combinedResults = Result.Combine(contentResult, titleResult);
        if (!combinedResults.IsSuccess)
        {
            return Result<CreatedPostDto>.SetTypeToFailedResult(combinedResults);
        }
        var post = new Post(titleResult.Value!, contentResult.Value, createPostDto.ImagePath);
        await _applicationDbContext.Posts.AddAsync(post, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        CreatedPostDto createdPostDto = _mapDispatcher.DispatchMap<Post, CreatedPostDto>(post);
        return Result<CreatedPostDto>.Succeed(createdPostDto);
    }
}
