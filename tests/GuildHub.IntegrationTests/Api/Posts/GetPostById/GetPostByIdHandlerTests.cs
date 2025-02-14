namespace GuildHub.IntegrationTests.Api.Posts.GetPostById;

[Collection(nameof(SharedDatabaseFixture))]
public sealed class GetPostByIdHandlerTests : IntegrationTest
{
    private readonly IRequestHandler<GetPostByIdDto, RetrievedPostByIdDto> _getPostByIdHandler;
    public GetPostByIdHandlerTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory) : base(integrationTestsWebApplicationFactory)
    {
        _getPostByIdHandler = ServiceProvider.GetRequiredService<IRequestHandler<GetPostByIdDto, RetrievedPostByIdDto>>();
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostIsNull_ShouldReturnFailedResultWithErrorMessage()
    {
        // Arrange:
        Guid invalidId = Guid.NewGuid();
        Result<RetrievedPostByIdDto> expectedResult = Result<RetrievedPostByIdDto>.Fail($"No post with the ID '{invalidId}' was found.");

        // Act:
        Result<RetrievedPostByIdDto> actualResult = await _getPostByIdHandler.HandleAsync(new(invalidId), It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostIsNotNull_ShouldReturnSuccessfulResult()
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;
        await ApplicationDbContext.Posts.AddAsync(post);
        await ApplicationDbContext.SaveChangesAsync();
        Result<RetrievedPostByIdDto> expectedResult = Result<RetrievedPostByIdDto>.Succeed(new(
            post.Id,
            post.Title.ToString(),
            post.Content!.ToString(),
            post.ImagePath,
            [
                ..
                post.PostReplies.Select(postReply => new RetrievedPostReplyForPostDto(
                    postReply.Content.ToString(),
                    postReply.ImagePath,
                    postReply.CreatedAtUtc))
            ],
            post.CreatedAtUtc,
            post.UpdatedAtUtc));

        // Act:
        Result<RetrievedPostByIdDto> actualResult = await _getPostByIdHandler.HandleAsync(new(post.Id), It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}
