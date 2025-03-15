using GuildHub.Api.Posts.PostReplies.UpdatePostReplyById;

namespace GuildHub.UnitTests.Api.Posts.PostReplies.UpdatePostReplyById;

public sealed class UpdatePostReplyByIdEndpointTests
{
    private readonly Mock<IRequestDispatcher> _requestDispatcherMock;

    public UpdatePostReplyByIdEndpointTests()
    {
        _requestDispatcherMock = new();
    }

    [Fact]
    public async Task UpdatePostReplyByIdAsync_WhenRequestDispatcherReturnsSuccessfulResult_ShouldReturnNoContent()
    {
        // Arrange:
        _requestDispatcherMock
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync(It.IsAny<UpdatePostReplyByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Succeed());

        // Act:
        Results<ProblemHttpResult, NoContent> actualResult = await UpdatePostReplyByIdEndpoint.UpdatePostReplyByIdAsync(
            _requestDispatcherMock.Object,
            It.IsAny<HttpContext>(),
            It.IsAny<Guid>(),
            It.IsAny<Guid>(),
            It.IsAny<UpdatePostReplyByIdDto>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeOfType<NoContent>();
    }

    [Fact]
    public async Task UpdatePostReplyByIdAsync_WhenRequestDispatcherReturnsPostNotFoundResult_ShouldReturnProblemHttpResultWithError()
    {
        // Arrange:
        const string ExpectedTracerIdentifier = "Identifier";
        Guid postId = Guid.NewGuid();
        var expectedErrorMessage = $"No post with the ID '{postId}' was found.";
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
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync(It.IsAny<UpdatePostReplyByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(expectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, NoContent> actualResult = await UpdatePostReplyByIdEndpoint.UpdatePostReplyByIdAsync(
            _requestDispatcherMock.Object,
            defaultHttpContext,
            postId,
            It.IsAny<Guid>(),
            It.IsAny<UpdatePostReplyByIdDto>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }

    [Fact]
    public async Task UpdatePostReplyByIdAsync_WhenRequestDispatcherReturnsPostReplyNotFoundResult_ShouldReturnProblemHttpResultWithError()
    {
        // Arrange:
        const string ExpectedTracerIdentifier = "Identifier";
        Guid id = Guid.NewGuid();
        var expectedErrorMessage = $"No post reply with the ID '{id}' was found.";
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
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync(It.IsAny<UpdatePostReplyByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(expectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, NoContent> actualResult = await UpdatePostReplyByIdEndpoint.UpdatePostReplyByIdAsync(
            _requestDispatcherMock.Object,
            defaultHttpContext,
            It.IsAny<Guid>(),
            id,
            It.IsAny<UpdatePostReplyByIdDto>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }

    [Fact]
    public async Task UpdatePostReplyByIdAsync_WhenRequestDispatcherReturnsFailedEntityResult_ShouldReturnProblemHttpResultWithError()
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
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync(It.IsAny<UpdatePostReplyByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(ExpectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, NoContent> actualResult = await UpdatePostReplyByIdEndpoint.UpdatePostReplyByIdAsync(
            _requestDispatcherMock.Object,
            defaultHttpContext,
            It.IsAny<Guid>(),
            It.IsAny<Guid>(),
            It.IsAny<UpdatePostReplyByIdDto>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }
}
