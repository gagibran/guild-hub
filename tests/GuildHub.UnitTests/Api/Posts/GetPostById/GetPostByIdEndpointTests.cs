namespace GuildHub.UnitTests.Api.Posts.GetPostById;

public sealed class GetPostByIdEndpointTests
{
    private readonly Mock<IRequestDispatcher> _requestDispatcherMock;

    public GetPostByIdEndpointTests()
    {
        _requestDispatcherMock = new();
    }

    [Fact]
    public async Task GetPostByIdAsync_WhenDispatcherReturnsFailedResult_ShouldReturnProblemHttpResultWithError()
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
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync<GetPostByIdDto, RetrievedPostByIdDto>(It.IsAny<GetPostByIdDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<RetrievedPostByIdDto>(ExpectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, Ok<RetrievedPostByIdDto>> actualResult = await GetPostByIdEndpoint.GetPostByIdAsync(
            _requestDispatcherMock.Object,
            defaultHttpContext,
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }

    [Fact]
    public async Task GetPostByIdAsync_WhenRequestDispatcherReturnsSuccessfulResult_ShouldReturnOkWithData()
    {
        // Arrange:
        var expectedRetrievedPostByIdDto = new RetrievedPostByIdDto(
            "Title",
            "Content",
            "ImagePath",
            [
                new("Message1", "ImagePath1", new DateTime(2020, 1, 1)),
                new("Message2", "ImagePath2", new DateTime(2020, 1, 2))
            ],
            new DateTime(2020, 1, 1));
        Ok<RetrievedPostByIdDto> expectedOk = TypedResults.Ok(expectedRetrievedPostByIdDto);
        _requestDispatcherMock
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync<GetPostByIdDto, RetrievedPostByIdDto>(It.IsAny<GetPostByIdDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedRetrievedPostByIdDto));

        // Act:
        Results<ProblemHttpResult, Ok<RetrievedPostByIdDto>> actualResult = await GetPostByIdEndpoint.GetPostByIdAsync(
            _requestDispatcherMock.Object,
            It.IsAny<HttpContext>(),
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedOk);
    }
}
