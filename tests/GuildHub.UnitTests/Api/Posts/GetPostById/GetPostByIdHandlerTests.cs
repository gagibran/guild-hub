namespace GuildHub.UnitTests.Api.Posts.GetPostById;

public class GetPostByIdHandlerTests
{
    private readonly Mock<IApplicationDbContext> _applicationDbContextMock;
    private readonly Mock<DbSet<Post>> _postDbSetMock;
    private readonly Mock<IMapDispatcher> _mapDispatcherMock;
    private readonly GetPostByIdHandler _getPostByIdHandler;

    public GetPostByIdHandlerTests()
    {
        _applicationDbContextMock = new();
        _postDbSetMock = new();
        _mapDispatcherMock = new();
        _getPostByIdHandler = new(_applicationDbContextMock.Object, _mapDispatcherMock.Object);
        _applicationDbContextMock
            .Setup(applicationDbContext => applicationDbContext.Posts)
            .Returns(_postDbSetMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostIsNull_ShouldReturnFailureWithErrorMessage()
    {
        // Arrange:
        var getPostByIdDto = new GetPostByIdDto(Guid.NewGuid());
        Result<RetrievedPostByIdDto> expectedRetrievedPostByIdDtoResult = Result.Fail<RetrievedPostByIdDto>($"No post with the ID '{getPostByIdDto.Id}' was found.");
        _postDbSetMock
            .Setup(postDbSet => postDbSet.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Post?>());

        // Act:
        Result<RetrievedPostByIdDto> actualRetrievedPostByIdDtoResult = await _getPostByIdHandler.HandleAsync(
            getPostByIdDto,
            It.IsAny<CancellationToken>());

        // Assert:
        actualRetrievedPostByIdDtoResult.Should().BeEquivalentTo(expectedRetrievedPostByIdDtoResult);
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostIsNotNull_ShouldReturnSuccessWithDto()
    {
        // Arrange:
        var expectedRetrievedPostByIdDto = new RetrievedPostByIdDto(
            "Title",
            "Content",
            "ImagePath",
            [new("Message", "ImagePath"), new("Message2", "ImagePath2")]);
        Result<RetrievedPostByIdDto> expectedRetrievedPostByIdDtoResult = Result.Success(expectedRetrievedPostByIdDto);
        _postDbSetMock
            .Setup(postDbSet => postDbSet.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Post?>(new Post(Title.Build("Title").Value!, "Content", "ImagePath")));
        _mapDispatcherMock
            .Setup(mapDispatcher => mapDispatcher.DispatchMap<Post, RetrievedPostByIdDto>(It.IsAny<Post>()))
            .Returns(expectedRetrievedPostByIdDto);

        // Act:
        Result<RetrievedPostByIdDto> actualRetrievedPostByIdDtoResult = await _getPostByIdHandler.HandleAsync(
            new(Guid.NewGuid()),
            It.IsAny<CancellationToken>());

        // Assert:
        actualRetrievedPostByIdDtoResult.Should().BeEquivalentTo(expectedRetrievedPostByIdDtoResult);
    }
}