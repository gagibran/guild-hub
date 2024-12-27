namespace GuildHub.UnitTests.Api.Posts.GetPosts;

public class GetPostsEndpointTests
{
    private readonly Mock<IRequestDispatcher> _requestDispatcherMock;

    public GetPostsEndpointTests()
    {
        _requestDispatcherMock = new();
    }

    [Fact]
    public async Task GetPostsAsync_WhenDispatcherReturnsFailedResult_ShouldReturnProblemHttpResultWithError()
    {
        // Arrange:
        const string ExpectedTracerIdentifier = "Identifier";
        const string ExpectedErrorMessage = "Entity error.";
        var defaultHttpContext = new DefaultHttpContext
        {
            TraceIdentifier = ExpectedTracerIdentifier
        };
        ProblemHttpResult expectedProblemHttpResult = TypedResults.Problem(
            title: "One or more validation errors occurred.",
            statusCode: (int)HttpStatusCode.UnprocessableEntity,
            extensions: new Dictionary<string, object?>
            {
                { "errors", new List<string> { ExpectedErrorMessage } },
                { "traceId", ExpectedTracerIdentifier }
            });
        _requestDispatcherMock
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync<GetPostsDto, RetrievedPostsDto>(It.IsAny<GetPostsDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<RetrievedPostsDto>(ExpectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, Ok<List<RetrievedPostByIdDto>>> actualResult = await GetPostsEndpoint.GetPostsAsync(
            It.IsAny<string?>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            _requestDispatcherMock.Object,
            defaultHttpContext,
            It.IsAny<string>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }

    [Fact]
    public async Task GetPostsAsync_WhenRequestDispatcherReturnsSuccessfulResult_ShouldReturnOkWithData()
    {
        // Arrange:
        const int ExpectedCurrentPageIndex = 1;
        const int ExpectedEntitiesPerPage = 2;
        var defaultHttpContext = new DefaultHttpContext();
        var retrievedPosts = new List<RetrievedPostByIdDto>
        {
            new(
                "Title",
                "Content",
                "ImagePath",
                [
                    new("Message", "ImagePath"),
                    new("Message2", "ImagePath2")
                ]),
            new(
                "Title2",
                "Content2",
                "ImagePath2",
                [
                    new("Message3", "ImagePath3"),
                    new("Message4", "ImagePath4")
                ]),
            new(
                "Title3",
                "Content3",
                "ImagePath3",
                [
                    new("Message5", "ImagePath5"),
                    new("Message6", "ImagePath6")
                ])
        };
        int expectedEntitiesCount = retrievedPosts.Count;
        int expectedPagesCount = (int)Math.Ceiling(expectedEntitiesCount / (double)ExpectedEntitiesPerPage);
        var retrievedPostsDto = new RetrievedPostsDto(
            retrievedPosts,
            ExpectedCurrentPageIndex,
            ExpectedEntitiesPerPage,
            expectedEntitiesCount,
            expectedPagesCount);
        Ok<List<RetrievedPostByIdDto>> expectedResult = TypedResults.Ok(retrievedPosts);
        _requestDispatcherMock
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync<GetPostsDto, RetrievedPostsDto>(It.IsAny<GetPostsDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(retrievedPostsDto));

        // Act:
        Results<ProblemHttpResult, Ok<List<RetrievedPostByIdDto>>> actualResult = await GetPostsEndpoint.GetPostsAsync(
            It.IsAny<string?>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            _requestDispatcherMock.Object,
            defaultHttpContext,
            It.IsAny<string>(),
            It.IsAny<CancellationToken>());

        // Assert:
        defaultHttpContext.Response.Headers["X-Pagination"]
            .Should()
            .Equal($"{{\"currentPageIndex\":{ExpectedCurrentPageIndex},\"entitiesPerPage\":{ExpectedEntitiesPerPage},\"entitiesCount\":{expectedEntitiesCount},\"pagesCount\":{expectedPagesCount}}}");
        actualResult.Result.Should().BeEquivalentTo(expectedResult);
    }
}
