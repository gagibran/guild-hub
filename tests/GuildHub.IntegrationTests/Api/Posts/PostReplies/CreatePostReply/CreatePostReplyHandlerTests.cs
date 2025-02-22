using GuildHub.Api.Posts.PostReplies.CreatePostReply;

namespace GuildHub.IntegrationTests.Api.Posts.PostReplies.CreatePostReply;

[Collection(nameof(SharedDatabaseFixture))]
public sealed class CreatePostReplyHandlerTests : IntegrationTest
{
    private readonly IRequestHandler<CreatePostReplyRequest, CreatedPostReplyDto> _createPostReplyHandler;

    public CreatePostReplyHandlerTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
        : base(integrationTestsWebApplicationFactory)
    {
        _createPostReplyHandler = ServiceProvider.GetRequiredService<IRequestHandler<CreatePostReplyRequest, CreatedPostReplyDto>>();
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostIsNull_ShouldReturnFailedResult()
    {
        // Arrange:
        Guid postId = Guid.NewGuid();
        Result<CreatedPostReplyDto> expectedResult = Result<CreatedPostReplyDto>.Fail($"No post with the ID '{postId}' was found.");

        // Act:
        Result<CreatedPostReplyDto> actualResult = await _createPostReplyHandler.HandleAsync(
            new(postId, It.IsAny<string>(), It.IsAny<string>()),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenPostReplyResultFails_ShouldReturnFailedResult()
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;
        await ApplicationDbContext.Posts.AddAsync(post);
        await ApplicationDbContext.SaveChangesAsync();
        Result<CreatedPostReplyDto> expectedResult = Result<CreatedPostReplyDto>.Fail("The content message cannot be null nor empty.");

        // Act:
        Result<CreatedPostReplyDto> actualResult = await _createPostReplyHandler.HandleAsync(
            new(post.Id, It.IsAny<string>(), It.IsAny<string>()),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenNoFailuresOccur_ShouldReturnSuccessfulResult()
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;
        await ApplicationDbContext.Posts.AddAsync(post);
        await ApplicationDbContext.SaveChangesAsync();

        // Act:
        Result<CreatedPostReplyDto> actualResult = await _createPostReplyHandler.HandleAsync(
            new(post.Id, "Content", "ImagePath"),
            It.IsAny<CancellationToken>());

        // Assert:
        Result<CreatedPostReplyDto> expectedResult = await ApplicationDbContext.PostReplies
            .Where(postReply => postReply.Id == actualResult.Value!.Id)
            .Select(postReply => Result<CreatedPostReplyDto>.Succeed(new(
                postReply.Id,
                postReply.Content!.ToString(),
                postReply.ImagePath)))
            .SingleAsync();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}
