namespace GuildHub.UnitTests.Api.Posts.UpdatePostById;

public sealed class UpdatePostByIdHandlerTests
{
    private readonly Mock<IApplicationDbContext> _applicationDbContextMock;
    private readonly Mock<DbSet<Post>> _postDbSetMock;
    private readonly UpdatePostByIdHandler _updatePostByIdHandler;

    public UpdatePostByIdHandlerTests()
    {
        _applicationDbContextMock = new();
        _postDbSetMock = new();
        _updatePostByIdHandler = new UpdatePostByIdHandler(_applicationDbContextMock.Object);
        _applicationDbContextMock
            .Setup(applicationDbContext => applicationDbContext.Posts)
            .Returns(_postDbSetMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostIsNull_ShouldReturnFailureWithErrorMessage()
    {
        // Arrange:
        var updatePostByIdRequest = new UpdatePostByIdRequest(Guid.NewGuid(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<string?>());
        Result expectedResult = Result.Fail($"No post with the ID '{updatePostByIdRequest.Id}' was found.");
        _postDbSetMock
            .Setup(postDbSet => postDbSet.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Post?>());

        // Act:
        Result actualResult = await _updatePostByIdHandler.HandleAsync(updatePostByIdRequest, It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenUpdatePostFails_ShouldReturnFailureWithErrorMessage()
    {
        // Arrange:
        var updatePostByIdRequest = new UpdatePostByIdRequest(It.IsAny<Guid>(), string.Empty, It.IsAny<string?>(), It.IsAny<string?>());
        var retrievedPost = new Post(Title.Build("Title").Value!, Content.Build("Content").Value, "ImagePath");
        Result expectedResult = Result.Fail("The title cannot be empty.");
        _postDbSetMock
            .Setup(postDbSet => postDbSet.FindAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Post?>(retrievedPost));

        // Act:
        Result actualResult = await _updatePostByIdHandler.HandleAsync(updatePostByIdRequest, It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenUpdatesSucceeds_ShouldReturnSuccess()
    {
        // Arrange:
        var retrievedPost = new Post(Title.Build("Title").Value!, Content.Build("Content").Value!, "ImagePath");
        _postDbSetMock
            .Setup(postDbSet => postDbSet.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Post?>(retrievedPost));

        // Act:
        Result actualResult = await _updatePostByIdHandler.HandleAsync(
            new(
                It.IsAny<Guid>(),
                "New Content",
                It.IsAny<string?>(),
                It.IsAny<string?>()),
            It.IsAny<CancellationToken>());

        // Assert:
        _applicationDbContextMock.Verify(
            applicationDbContext => applicationDbContext.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        actualResult.Should().BeEquivalentTo(Result.Succeed());
    }
}
