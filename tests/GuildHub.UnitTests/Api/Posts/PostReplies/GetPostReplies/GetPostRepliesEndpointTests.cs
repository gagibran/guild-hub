using GuildHub.Api.Posts.PostReplies.GetPostReplies;

namespace GuildHub.UnitTests.Api.Posts.PostReplies.GetPostReplies;

public sealed class GetPostRepliesEndpointTests
{
    private readonly Mock<IRequestDispatcher> _requestDispatcherMock;

    public GetPostRepliesEndpointTests()
    {
        _requestDispatcherMock = new Mock<IRequestDispatcher>();
    }

    [Fact]
    public async Task GetPostRepliesAsync_WhenRequestDispatcherReturnsSuccessfulResult_ShouldReturnOkWithData()
    {
        // Arrange:
        const int ExpectedCurrentPageIndex = 1;
        const int ExpectedEntitiesPerPage = 2;
        var defaultHttpContext = new DefaultHttpContext();
        var retrievedReplies = new List<RetrievedPostReplyByIdDto>
        {
            new(Guid.NewGuid(), "Content", "Author", DateTime.Now, DateTime.Now),
            new(Guid.NewGuid(), "Another Content", "Another Author", DateTime.Now, DateTime.Now)
        };
        int expectedEntitiesCount = retrievedReplies.Count;
        int expectedPagesCount = (int)Math.Ceiling(expectedEntitiesCount / (double)ExpectedEntitiesPerPage);
        _requestDispatcherMock
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync<GetPostRepliesRequest, RetrievedPostRepliesDto>(
                It.IsAny<GetPostRepliesRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<RetrievedPostRepliesDto>.Succeed(new(
                retrievedReplies,
                ExpectedCurrentPageIndex,
                ExpectedEntitiesPerPage,
                expectedEntitiesCount,
                expectedPagesCount)));
        Ok<List<RetrievedPostReplyByIdDto>> expectedResult = TypedResults.Ok(retrievedReplies);

        // Act:
        Results<ProblemHttpResult, Ok<List<RetrievedPostReplyByIdDto>>> actualResult = await GetPostRepliesEndpoint.GetPostRepliesAsync(
            It.IsAny<Guid>(),
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

    [Fact]
    public async Task GetPostRepliesAsync_WhenDispatcherReturnsFailedResult_ShouldReturnProblemHttpResultWithError()
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
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync<GetPostRepliesRequest, RetrievedPostRepliesDto>(
                It.IsAny<GetPostRepliesRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<RetrievedPostRepliesDto>.Fail(ExpectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, Ok<List<RetrievedPostReplyByIdDto>>> actualResult = await GetPostRepliesEndpoint.GetPostRepliesAsync(
            It.IsAny<Guid>(),
            It.IsAny<QueryParameters>(),
            _requestDispatcherMock.Object,
            defaultHttpContext,
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }

    [Fact]
    public async Task GetPostRepliesAsync_WhenNoPostIsFound_ShouldReturnProblemHttpResultWith404Error()
    {
        // Arrange:
        const string ExpectedTracerIdentifier = "Identifier";
        var postId = Guid.NewGuid();
        var expectedErrorMessage = $"Post with ID '{postId}' not found.";
        var defaultHttpContext = new DefaultHttpContext
        {
            TraceIdentifier = ExpectedTracerIdentifier
        };
        ProblemHttpResult expectedProblemHttpResult = TypedResults.Problem(
            title: "One or more validation errors occurred.",
            statusCode: (int)HttpStatusCode.NotFound,
            extensions: new Dictionary<string, object?>
            {
                { "errors", new List<string> { expectedErrorMessage } },
                { "traceId", ExpectedTracerIdentifier }
            });
        _requestDispatcherMock
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync<GetPostRepliesRequest, RetrievedPostRepliesDto>(
                It.IsAny<GetPostRepliesRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<RetrievedPostRepliesDto>.Fail(expectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, Ok<List<RetrievedPostReplyByIdDto>>> actualResult = await GetPostRepliesEndpoint.GetPostRepliesAsync(
            postId,
            It.IsAny<QueryParameters>(),
            _requestDispatcherMock.Object,
            defaultHttpContext,
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }
}
