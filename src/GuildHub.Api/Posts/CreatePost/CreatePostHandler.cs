using GuildHub.Api.Common.RequestHandler;
using GuildHub.Api.Data;

namespace GuildHub.Api.Posts.CreatePost;

public sealed class CreatePostHandler(IApplicationDbContext applicationDbContext) : IRequestHandler<CreatePostDto, PostCreatedDto>
{
    private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<PostCreatedDto> HandleAsync(CreatePostDto createPostDto, CancellationToken cancellationToken)
    {
        var post = new Post(createPostDto.Title, createPostDto.Content, createPostDto.ImagePath);
        _applicationDbContext.Posts.Add(post);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return new PostCreatedDto(createPostDto.Title, createPostDto.Content, createPostDto.ImagePath);
    }
}
