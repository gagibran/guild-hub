namespace GuildHub.Api.Posts.PostReplies.CreatePostReply;

public sealed class CreatePostReplyHandler(IApplicationDbContext applicationDbContext) : IRequestHandler<CreatePostReplyRequest>
{
    private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<Result> HandleAsync(CreatePostReplyRequest createPostReplyDto, CancellationToken cancellationToken)
    {
        Post? retrievedPost = await _applicationDbContext.Posts.FindAsync(createPostReplyDto.PostId);
        if (retrievedPost is null)
        {
            return Result.Fail($"No post with the ID '{createPostReplyDto.PostId}' was found.");
        }
        Result<PostReply> postReplyResult = PostReply.Build(retrievedPost, createPostReplyDto.Content, createPostReplyDto.ImagePath);
        if (!postReplyResult.IsSuccess)
        {
            return Result.Fail(postReplyResult.Errors);
        }
        retrievedPost.AddPostReply(postReplyResult.Value!);
        _applicationDbContext.PostReplies.Add(postReplyResult.Value!);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return Result.Succeed();
    }
}
