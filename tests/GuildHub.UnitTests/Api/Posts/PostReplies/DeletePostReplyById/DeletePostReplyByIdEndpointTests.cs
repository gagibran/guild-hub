using GuildHub.Api.Posts.PostReplies.DeletePostReplyById;

namespace GuildHub.UnitTests.Api.Posts.PostReplies.DeletePostReplyById;

public sealed class DeletePostReplyByIdEndpointTests
{
    private readonly Mock<IRequestDispatcher> _requestDispatcherMock;

    public DeletePostReplyByIdEndpointTests()
    {
        _requestDispatcherMock = new();
    }

    [Fact]
    public async Task DeletePostReplyByIdAsync_WhenRequestDispatcherReturnsFailedResult_ShouldReturnProblemHttpResultWithError()
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
            statusCode: (int)HttpStatusCode.NotFound,
            extensions: new Dictionary<string, object?>
            {
                { "errors", new List<string> { ExpectedErrorMessage } },
                { "traceId", ExpectedTracerIdentifier }
            });
        _requestDispatcherMock
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync(It.IsAny<DeletePostReplyByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(ExpectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, NoContent> actualResult = await DeletePostReplyByIdEndpoint.DeletePostReplyByIdAsync(
            _requestDispatcherMock.Object,
            defaultHttpContext,
            It.IsAny<Guid>(),
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }

    [Fact]
    public async Task DeletePostReplyByIdAsync_WhenRequestDispatcherReturnsSuccessfulResult_ShouldReturnNoContent()
    {
        // Arrange:
        _requestDispatcherMock
            .Setup(dispatcher => dispatcher.DispatchRequestAsync(It.IsAny<DeletePostReplyByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Succeed());

        // Act:
        Results<ProblemHttpResult, NoContent> actualResult = await DeletePostReplyByIdEndpoint.DeletePostReplyByIdAsync(
            _requestDispatcherMock.Object,
            It.IsAny<HttpContext>(),
            It.IsAny<Guid>(),
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeOfType<NoContent>();
    }
}
