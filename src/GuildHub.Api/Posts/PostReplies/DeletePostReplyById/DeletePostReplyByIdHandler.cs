namespace GuildHub.Api.Posts.PostReplies.DeletePostReplyById;

public sealed class DeletePostReplyByIdHandler(ApplicationDbContext applicationDbContext) : IRequestHandler<DeletePostReplyByIdRequest>
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<Result> HandleAsync(DeletePostReplyByIdRequest deletePostReplyByIdRequest, CancellationToken cancellationToken)
    {
        var retrievedPost = await _applicationDbContext.Posts
            .Where(post => post.Id == deletePostReplyByIdRequest.PostId)
            .Select(post => new
            {
                PostReply = post.PostReplies.SingleOrDefault(postReply => postReply.Id == deletePostReplyByIdRequest.Id)
            })
            .SingleOrDefaultAsync(cancellationToken);
        if (retrievedPost is null)
        {
            return Result.Fail($"No post with the ID '{deletePostReplyByIdRequest.PostId}' was found.");
        }
        if (retrievedPost.PostReply is null)
        {
            return Result.Fail($"No post reply with the ID '{deletePostReplyByIdRequest.Id}' was found.");
        }
        _applicationDbContext.PostReplies.Remove(retrievedPost.PostReply);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return Result.Succeed();
    }
}
