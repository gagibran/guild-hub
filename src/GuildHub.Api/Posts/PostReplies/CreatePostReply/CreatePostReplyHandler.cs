namespace GuildHub.Api.Posts.PostReplies.CreatePostReply;

public sealed class CreatePostReplyHandler(ApplicationDbContext applicationDbContext, IMapHandler<PostReply, CreatedPostReplyDto> postReplyToCreatedPostReplyDtoMapper)
    : IRequestHandler<CreatePostReplyRequest, CreatedPostReplyDto>
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly IMapHandler<PostReply, CreatedPostReplyDto> _postReplyToCreatedPostReplyDtoMapper = postReplyToCreatedPostReplyDtoMapper;

    public async Task<Result<CreatedPostReplyDto>> HandleAsync(CreatePostReplyRequest createPostReplyRequest, CancellationToken cancellationToken)
    {
        Post? retrievedPost = await _applicationDbContext.Posts.FindAsync(createPostReplyRequest.PostId);
        if (retrievedPost is null)
        {
            return Result<CreatedPostReplyDto>.Fail($"No post with the ID '{createPostReplyRequest.PostId}' was found.");
        }
        Result<PostReply> postReplyResult = PostReply.Build(retrievedPost, createPostReplyRequest.Content, createPostReplyRequest.ImagePath);
        if (!postReplyResult.IsSuccess)
        {
            return Result<CreatedPostReplyDto>.Fail(postReplyResult.Errors);
        }
        retrievedPost.AddPostReply(postReplyResult.Value!);
        await _applicationDbContext.PostReplies.AddAsync(postReplyResult.Value!, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return Result<CreatedPostReplyDto>.Succeed(_postReplyToCreatedPostReplyDtoMapper.Map(postReplyResult.Value!));
    }
}
