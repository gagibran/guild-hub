namespace GuildHub.Api.Posts.DeletePostById;

public sealed class DeletePostByIdHandler(ApplicationDbContext applicationDbContext) : IRequestHandler<DeletePostByIdRequest>
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<Result> HandleAsync(DeletePostByIdRequest deletePostByIdRequest, CancellationToken cancellationToken)
    {
        Post? retrievedPost = await _applicationDbContext.Posts.FindAsync(deletePostByIdRequest.Id, cancellationToken);
        if (retrievedPost is null)
        {
            return Result.Fail($"No post with the ID '{deletePostByIdRequest.Id}' was found.");
        }
        _applicationDbContext.Posts.Remove(retrievedPost);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return Result.Succeed();
    }
}
