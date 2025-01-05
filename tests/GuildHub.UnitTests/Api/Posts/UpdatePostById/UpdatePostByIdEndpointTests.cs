namespace GuildHub.UnitTests.Api.Posts.UpdatePostById;

public sealed class UpdatePostByIdEndpointTests
{
    private readonly Mock<IRequestDispatcher> _requestDispatcherMock;

    public UpdatePostByIdEndpointTests()
    {
        _requestDispatcherMock = new();
    }

    [Fact]
    public async Task UpdatePostByIdAsync_WhenRequestDispatcherReturnsSuccessfulResult_ShouldReturnNoContent()
    {
        // Arrange:
        _requestDispatcherMock
            .Setup(dispatcher => dispatcher.DispatchRequestAsync(It.IsAny<UpdatePostByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Succeed());

        // Act:
        Results<ProblemHttpResult, NoContent> actualResult = await UpdatePostByIdEndpoint.UpdatePostByIdAsync(
            _requestDispatcherMock.Object,
            It.IsAny<HttpContext>(),
            It.IsAny<Guid>(),
            new(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<string?>()),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeOfType<NoContent>();
    }

    [Fact]
    public async Task UpdatePostByIdAsync_WhenRequestDispatcherReturnsNotFoundResult_ShouldReturnProblemHttpResultWithError()
    {
        // Arrange:
        const string ExpectedTracerIdentifier = "Identifier";
        Guid id = Guid.NewGuid();
        var expectedErrorMessage = $"No post with the ID '{id}' was found.";
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
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync(It.IsAny<UpdatePostByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(expectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, NoContent> actualResult = await UpdatePostByIdEndpoint.UpdatePostByIdAsync(
            _requestDispatcherMock.Object,
            defaultHttpContext,
            id,
            new(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<string?>()),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }

    [Fact]
    public async Task UpdatePostByIdAsync_WhenRequestDispatcherReturnsFailedEntityResult_ShouldReturnProblemHttpResultWithError()
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
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync(It.IsAny<UpdatePostByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(ExpectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, NoContent> actualResult = await UpdatePostByIdEndpoint.UpdatePostByIdAsync(
            _requestDispatcherMock.Object,
            defaultHttpContext,
            It.IsAny<Guid>(),
            new(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<string?>()),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }
}
