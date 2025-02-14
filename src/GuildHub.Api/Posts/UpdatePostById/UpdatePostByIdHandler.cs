namespace GuildHub.Api.Posts.UpdatePostById;

public sealed class UpdatePostByIdHandler(ApplicationDbContext applicationDbContext) : IRequestHandler<UpdatePostByIdRequest>
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<Result> HandleAsync(UpdatePostByIdRequest updatePostByIdRequest, CancellationToken cancellationToken)
    {
        Post? retrievedPost = await _applicationDbContext.Posts.FindAsync(updatePostByIdRequest.Id, cancellationToken);
        if (retrievedPost is null)
        {
            return Result.Fail($"No post with the ID '{updatePostByIdRequest.Id}' was found.");
        }
        Result updatePostResult = retrievedPost.Update(
            updatePostByIdRequest.Title,
            updatePostByIdRequest.Content,
            updatePostByIdRequest.ImagePath);
        if (!updatePostResult.IsSuccess)
        {
            return updatePostResult;
        }
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return Result.Succeed();
    }
}
