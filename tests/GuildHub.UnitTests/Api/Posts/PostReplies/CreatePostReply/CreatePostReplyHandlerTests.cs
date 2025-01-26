namespace GuildHub.UnitTests.Api.Posts.PostReplies.CreatePostReply;

public class CreatePostReplyHandlerTests
{
    private readonly Mock<IApplicationDbContext> _applicationDbContextMock;
    private readonly CreatePostReplyHandler _createPostReplyHandler;

    public CreatePostReplyHandlerTests()
    {
        _applicationDbContextMock = new Mock<IApplicationDbContext>();
        _createPostReplyHandler = new CreatePostReplyHandler(_applicationDbContextMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenPostDoesNotExist_ReturnsFailedResultWithErrorMessage()
    {
        // Arrange:
        Guid guid = Guid.NewGuid();
        _applicationDbContextMock
            .Setup(applicationDbContext => applicationDbContext.Posts.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Post?)null);
        Result expectedResult = Result.Fail($"No post with the ID '{guid}' was found.");

        // Act:
        Result actualResult = await _createPostReplyHandler.HandleAsync(
            new(guid, It.IsAny<string>(), It.IsAny<string>()),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenPostExistsAndPostReplyFailsToBuild_ReturnsFailedResultWithErrorMessage()
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;
        var createPostReplyRequest = new CreatePostReplyRequest(It.IsAny<Guid>(), string.Empty, "ImagePath");
        Result expectedResult = Result.Fail("The content message cannot be null nor empty.");
        _applicationDbContextMock
            .Setup(applicationDbContext => applicationDbContext.Posts.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(post);

        // Act:
        Result actualResult = await _createPostReplyHandler.HandleAsync(createPostReplyRequest, It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenPostExistsAndPostReplySucceeds_ReturnsSuccessfulResult()
    {
        // Arrange:
        var postRepliesMock = new Mock<DbSet<PostReply>>();
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;
        var createPostReplyRequest = new CreatePostReplyRequest(post.Id, "Content", "ImagePath");
        _applicationDbContextMock
            .Setup(applicationDbContext => applicationDbContext.Posts.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(post);
        _applicationDbContextMock
            .SetupGet(applicationDbContext => applicationDbContext.PostReplies)
            .Returns(postRepliesMock.Object);

        // Act:
        Result actualResult = await _createPostReplyHandler.HandleAsync(createPostReplyRequest, It.IsAny<CancellationToken>());

        // Assert:
        post.PostReplies.Should().HaveCount(1);
        postRepliesMock.Verify(postReplies => postReplies.Add(It.IsAny<PostReply>()), Times.Once);
        _applicationDbContextMock.Verify(
            applicationDbContext => applicationDbContext.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        actualResult.Should().BeEquivalentTo(Result.Succeed());
    }
}
