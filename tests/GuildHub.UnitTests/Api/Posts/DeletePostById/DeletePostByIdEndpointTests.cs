namespace GuildHub.UnitTests.Api.Posts.DeletePostById;

public sealed class DeletePostByIdEndpointTests
{
    private readonly Mock<IRequestDispatcher> _requestDispatcherMock;

    public DeletePostByIdEndpointTests()
    {
        _requestDispatcherMock = new();
    }

    [Fact]
    public async Task DeletePostByIdAsync_WhenRequestDispatcherReturnsFailedResult_ShouldReturnProblemHttpResultWithError()
    {
        // Arrange
        const string ExpectedTracerIdentifier = "Identifier";
        const string ExpectedErrorMessage = "Entity error.";
        var defaultHttpContext = new DefaultHttpContext
        {
            TraceIdentifier = ExpectedTracerIdentifier
        };
        ProblemHttpResult expectedProblemHttpResult = TypedResults.Problem(
            title: "One or more validation errors occurred.",
            statusCode: (int)HttpStatusCode.NotFound,
            extensions: new Dictionary<string, object?>
            {
                { "errors", new List<string> { ExpectedErrorMessage } },
                { "traceId", ExpectedTracerIdentifier }
            });
        _requestDispatcherMock
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync(It.IsAny<DeletePostByIdDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(ExpectedErrorMessage));

        // Act
        Results<ProblemHttpResult, NoContent> actualResult = await DeletePostByIdEndpoint.DeletePostByIdAsync(
            _requestDispatcherMock.Object,
            defaultHttpContext,
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }

    [Fact]
    public async Task DeletePostByIdAsync_WhenRequestDispatcherReturnsSuccessfulResult_ShouldReturnNoContent()
    {
        // Arrange:
        _requestDispatcherMock
            .Setup(dispatcher => dispatcher.DispatchRequestAsync(It.IsAny<DeletePostByIdDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Succeed());

        // Act:
        Results<ProblemHttpResult, NoContent> actualResult = await DeletePostByIdEndpoint.DeletePostByIdAsync(
            _requestDispatcherMock.Object,
            It.IsAny<HttpContext>(),
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeOfType<NoContent>();
    }
}
