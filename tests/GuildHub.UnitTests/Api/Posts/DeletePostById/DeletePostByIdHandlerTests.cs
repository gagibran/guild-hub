namespace GuildHub.UnitTests.Api.Posts.DeletePostById;

public sealed class DeletePostByIdHandlerTests
{
    private readonly Mock<IApplicationDbContext> _applicationDbContextMock;
    private readonly Mock<DbSet<Post>> _postDbSetMock;
    private readonly DeletePostByIdHandler _deletePostByIdHandler;

    public DeletePostByIdHandlerTests()
    {
        _applicationDbContextMock = new();
        _postDbSetMock = new();
        _deletePostByIdHandler = new(_applicationDbContextMock.Object);
        _applicationDbContextMock
            .Setup(applicationDbContext => applicationDbContext.Posts)
            .Returns(_postDbSetMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostIsNull_ShouldReturnFailureWithErrorMessage()
    {
        // Arrange:
        var deletePostByIdDto = new DeletePostByIdDto(Guid.NewGuid());
        Result expectedResult = Result.Fail($"No post with the ID '{deletePostByIdDto.Id}' was found.");
        _postDbSetMock
            .Setup(postDbSet => postDbSet.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Post?>());

        // Act:
        Result actualResult = await _deletePostByIdHandler.HandleAsync(deletePostByIdDto, It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostIsNotNull_ShouldReturnSuccessWithDto()
    {
        // Arrange:
        var expectedPost = new Post(Title.Build("Title").Value!, Content.Build("Content").Value!, "ImagePath");
        _postDbSetMock
            .Setup(postDbSet => postDbSet.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Post?>(expectedPost));

        // Act:
        Result actualResult = await _deletePostByIdHandler.HandleAsync(new(It.IsAny<Guid>()), It.IsAny<CancellationToken>());

        // Assert:
        _applicationDbContextMock.Verify(
            applicationDbContext => applicationDbContext.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        _postDbSetMock.Verify(postDbSet => postDbSet.Remove(expectedPost), Times.Once);
        actualResult.Should().BeEquivalentTo(Result.Succeed());
    }
}
