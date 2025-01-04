namespace GuildHub.UnitTests.Api.Posts.GetPosts;

public sealed class GetPostsHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenSortByIsNotValid_ShouldReturnFailedResultWithErrorMessage()
    {
        // Arrange:
        var getPostsHandler = new GetPostsHandler(new Mock<IApplicationDbContext>().Object, new Mock<IMapDispatcher>().Object);
        var getPostsDto = new GetPostsDto("", It.IsAny<int?>(), It.IsAny<int?>(), "Invalid sort by type");
        Result<RetrievedPostsDto> expectedResult = Result<RetrievedPostsDto>.Fail(
            $"Cannot sort by '{getPostsDto.SortBy}'. The valid options are: [{string.Join(", ", Enum.GetNames<SortByType>())}].");

        // Act:
        Result<RetrievedPostsDto> actualResult = await getPostsHandler.HandleAsync(getPostsDto, It.IsAny<CancellationToken>());

        // Assert:
        expectedResult.Should().BeEquivalentTo(actualResult);
    }
}
