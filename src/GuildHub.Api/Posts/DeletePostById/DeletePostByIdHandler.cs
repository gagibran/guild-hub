namespace GuildHub.Api.Posts.DeletePostById;

public sealed class DeletePostByIdHandler(IApplicationDbContext applicationDbContext) : IRequestHandler<DeletePostByIdDto>
{
    private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<Result> HandleAsync(DeletePostByIdDto deletePostByIdDto, CancellationToken cancellationToken)
    {
        Post? retrievedPost = await _applicationDbContext.Posts.FindAsync(deletePostByIdDto.Id, cancellationToken);
        if (retrievedPost is null)
        {
            return Result.Fail($"No post with the ID '{deletePostByIdDto.Id}' was found.");
        }
        _applicationDbContext.Posts.Remove(retrievedPost);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return Result.Succeed();
    }
}
