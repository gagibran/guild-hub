namespace GuildHub.UnitTests.Api.Posts.PostReplies.CreatePostReply;

public class CreatePostReplyEndpointTests
{
    private readonly Mock<IRequestDispatcher> _requestDispatcherMock;

    public CreatePostReplyEndpointTests()
    {
        _requestDispatcherMock = new();
    }

    [Fact]
    public async Task CreatePostReplyAsync_WhenRequestDispatcherReturnsSuccessfulResult_ShouldReturnCreated()
    {
        // Arrange:
        var createdPostReplyDto = new CreatedPostReplyDto(Guid.NewGuid(), "Content", "ImagePath");
        _requestDispatcherMock
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync<CreatePostReplyRequest, CreatedPostReplyDto>(
                It.IsAny<CreatePostReplyRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CreatedPostReplyDto>.Succeed(createdPostReplyDto));

        // Act:
        Results<ProblemHttpResult, Created<CreatedPostReplyDto>> actualResult = await CreatePostReplyEndpoint.CreatePostReplyAsync(
            _requestDispatcherMock.Object,
            It.IsAny<HttpContext>(),
            It.IsAny<Guid>(),
            new(It.IsAny<string>(), It.IsAny<string?>()),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.As<Created<CreatedPostReplyDto>>().Value.Should().BeEquivalentTo(createdPostReplyDto);
    }

    [Fact]
    public async Task CreatePostReplyAsync_WhenRequestDispatcherReturnsNotFound_ShouldReturnProblemHttpResultWithError()
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
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync<CreatePostReplyRequest, CreatedPostReplyDto>(
                It.IsAny<CreatePostReplyRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CreatedPostReplyDto>.Fail(expectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, Created<CreatedPostReplyDto>> actualResult = await CreatePostReplyEndpoint.CreatePostReplyAsync(
            _requestDispatcherMock.Object,
            defaultHttpContext,
            postId,
            new(It.IsAny<string>(), It.IsAny<string?>()),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }

    [Fact]
    public async Task CreatePostReplyAsync_WhenRequestDispatcherReturnsFailedResult_ShouldReturnProblemHttpResultWithError()
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
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync<CreatePostReplyRequest, CreatedPostReplyDto>(
                It.IsAny<CreatePostReplyRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CreatedPostReplyDto>.Fail(ExpectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, Created<CreatedPostReplyDto>> actualResult = await CreatePostReplyEndpoint.CreatePostReplyAsync(
            _requestDispatcherMock.Object,
            defaultHttpContext,
            It.IsAny<Guid>(),
            new(It.IsAny<string>(), It.IsAny<string?>()),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }
}
