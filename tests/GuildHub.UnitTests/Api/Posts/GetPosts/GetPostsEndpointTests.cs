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
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync<GetPostsRequest, RetrievedPostsDto>(It.IsAny<GetPostsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<RetrievedPostsDto>.Fail(ExpectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, Ok<List<RetrievedPostByIdDto>>> actualResult = await GetPostsEndpoint.GetPostsAsync(
            It.IsAny<QueryParameters>(),
            _requestDispatcherMock.Object,
            defaultHttpContext,
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
                Guid.NewGuid(),
                "Title",
                "Content",
                "ImagePath",
                "GetRepliesEndpoint",
                new DateTime(2022, 2, 3),
                new DateTime(2023, 2, 3)),
            new(
                Guid.NewGuid(),
                "Title2",
                "Content2",
                "ImagePath2",
                "GetRepliesEndpoint",
                new DateTime(2021, 2, 3),
                null),
            new(
                Guid.NewGuid(),
                "Title3",
                "Content3",
                "ImagePath3",
                "GetRepliesEndpoint",
                new DateTime(2021, 2, 3),
                new DateTime(2022, 2, 3))
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
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync<GetPostsRequest, RetrievedPostsDto>(It.IsAny<GetPostsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<RetrievedPostsDto>.Succeed(retrievedPostsDto));

        // Act:
        Results<ProblemHttpResult, Ok<List<RetrievedPostByIdDto>>> actualResult = await GetPostsEndpoint.GetPostsAsync(
            It.IsAny<QueryParameters>(),
            _requestDispatcherMock.Object,
            defaultHttpContext,
            It.IsAny<CancellationToken>());

        // Assert:
        defaultHttpContext.Response.Headers["X-Pagination"]
            .Should()
            .Equal($"{{\"currentPageIndex\":{ExpectedCurrentPageIndex},\"entitiesPerPage\":{ExpectedEntitiesPerPage},\"entitiesCount\":{expectedEntitiesCount},\"pagesCount\":{expectedPagesCount}}}");
        actualResult.Result.Should().BeEquivalentTo(expectedResult);
    }
}
