namespace GuildHub.UnitTests.Api.Posts.CreatePost;

public sealed class CreatePostEndpointTests
{
    private readonly Mock<IRequestDispatcher> _requestDispatcherMock;

    public CreatePostEndpointTests()
    {
        _requestDispatcherMock = new();
    }

    [Fact]
    public async Task CreatePostAsync_WhenRequestDispatcherReturnsFailedResult_ShouldReturnProblemHttpResultWithError()
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
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync<CreatePostDto, CreatedPostDto>(It.IsAny<CreatePostDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<CreatedPostDto>(ExpectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, CreatedAtRoute<CreatedPostDto>> actualResult = await CreatePostEndpoint.CreatePostAsync(
            _requestDispatcherMock.Object,
            defaultHttpContext,
            It.IsAny<CreatePostDto>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }

    [Fact]
    public async Task CreatePostAsync_WhenRequestDispatcherReturnsSuccessfulResult_ShouldReturnCreatedAtRouteWithData()
    {
        // Arrange:
        var expectedCreatedPostDto = new CreatedPostDto(Guid.NewGuid(), "Title", "Content", "ImagePath");
        CreatedAtRoute<CreatedPostDto> expectedCreatedAtRoute = TypedResults.CreatedAtRoute(
            expectedCreatedPostDto,
            nameof(GetPostByIdEndpoint.GetPostByIdAsync),
            new { expectedCreatedPostDto.Id });
        _requestDispatcherMock
            .Setup(requestDispatcher => requestDispatcher.DispatchRequestAsync<CreatePostDto, CreatedPostDto>(It.IsAny<CreatePostDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedCreatedPostDto));

        // Act:
        Results<ProblemHttpResult, CreatedAtRoute<CreatedPostDto>> actualResult = await CreatePostEndpoint.CreatePostAsync(
            _requestDispatcherMock.Object,
            It.IsAny<HttpContext>(),
            It.IsAny<CreatePostDto>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedCreatedAtRoute);
    }
}
