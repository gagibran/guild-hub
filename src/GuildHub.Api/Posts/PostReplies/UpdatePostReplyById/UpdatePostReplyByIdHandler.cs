namespace GuildHub.Api.Posts.PostReplies.UpdatePostReplyById;

public sealed class UpdatePostReplyByIdHandler(ApplicationDbContext applicationDbContext) : IRequestHandler<UpdatePostReplyByIdRequest>
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<Result> HandleAsync(UpdatePostReplyByIdRequest updatePostReplyByIdRequest, CancellationToken cancellationToken)
    {
        var retrievedPost = await _applicationDbContext.Posts
            .Where(post => post.Id == updatePostReplyByIdRequest.PostId)
            .Select(post => new
            {
                PostReply = post.PostReplies.SingleOrDefault(postReply => postReply.Id == updatePostReplyByIdRequest.Id)
            })
            .SingleOrDefaultAsync(cancellationToken);
        if (retrievedPost is null)
        {
            return Result.Fail($"No post with the ID '{updatePostReplyByIdRequest.PostId}' was found.");
        }
        if (retrievedPost.PostReply is null)
        {
            return Result.Fail($"No post reply with the ID '{updatePostReplyByIdRequest.Id}' was found.");
        }
        Result updatePostReplyResult = retrievedPost.PostReply.Update(
            updatePostReplyByIdRequest.UpdatePostReplyByIdDto.Content,
            updatePostReplyByIdRequest.UpdatePostReplyByIdDto.ImagePath);
        if (!updatePostReplyResult.IsSuccess)
        {
            return updatePostReplyResult;
        }
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return Result.Succeed();
    }
}
