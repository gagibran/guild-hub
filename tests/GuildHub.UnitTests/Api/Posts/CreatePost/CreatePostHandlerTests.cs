namespace GuildHub.UnitTests.Api.Posts.CreatePost;

public sealed class CreatePostHandlerTests
{
    private readonly Mock<IApplicationDbContext> _applicationDbContextMock;
    private readonly Mock<IMapDispatcher> _mapDispatcherMock;
    private readonly CreatePostHandler _createPostHandler;

    public CreatePostHandlerTests()
    {
        _applicationDbContextMock = new();
        _mapDispatcherMock = new();
        _createPostHandler = new(_applicationDbContextMock.Object, _mapDispatcherMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenPostTitleResultIsUnsuccessful_ShouldReturnFailureWithErrorMessage()
    {
        // Arrange:
        Result<CreatedPostDto> expectedCreatedPostDtoResult = Result.Fail<CreatedPostDto>("The post title cannot be empty.");
        var createPostDto = new CreatePostDto("", "Content", "ImagePath");

        // Act:
        Result<CreatedPostDto> actualCreatedPostDtoResult = await _createPostHandler.HandleAsync(createPostDto, It.IsAny<CancellationToken>());

        // Assert:
        actualCreatedPostDtoResult.Should().BeEquivalentTo(expectedCreatedPostDtoResult);
    }

    [Fact]
    public async Task HandleAsync_WhenPostTitleResultIsSuccessful_ShouldCallDatabaseMethodsAndReturnSuccessWithDto()
    {
        // Arrange:
        var postDbSetMock = new Mock<DbSet<Post>>();
        var createPostDto = new CreatePostDto("Title", "Content", "ImagePath");
        var expectedCreatedPostDto = new CreatedPostDto(Guid.NewGuid(), "Title", "Content", "ImagePath");
        Result<CreatedPostDto> expectedCreatedPostDtoResult = Result.Success(expectedCreatedPostDto);
        _applicationDbContextMock
            .SetupGet(applicationDbContext => applicationDbContext.Posts)
            .Returns(postDbSetMock.Object);
        _mapDispatcherMock
            .Setup(mapDispatcher => mapDispatcher.DispatchMap<Post, CreatedPostDto>(It.IsAny<Post>()))
            .Returns(expectedCreatedPostDto);

        // Act:
        Result<CreatedPostDto> actualCreatedPostDtoResult = await _createPostHandler.HandleAsync(createPostDto, It.IsAny<CancellationToken>());

        // Assert:
        postDbSetMock.Verify(
            postDbSet => postDbSet.AddAsync(
                It.Is<Post>(
                    post => post.Title == Title.Build("Title").Value!
                    && post.Content == "Content"
                    && post.ImagePath == "ImagePath"),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _applicationDbContextMock.Verify(
            applicationDbContext => applicationDbContext.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        actualCreatedPostDtoResult.Should().BeEquivalentTo(expectedCreatedPostDtoResult);
    }
}
