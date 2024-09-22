using System.Net;
using GuildHub.Common.RequestHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GuildHub.UnitTests.Posts.CreatePost;

public class CreatePostEndpointTests
{
    private readonly Mock<IRequestDispatcher> _dispatcherMock;

    public CreatePostEndpointTests()
    {
        _dispatcherMock = new();
    }

    [Fact]
    public async Task CreatePostAsync_WhenDispatcherReturnsFailedResult_ShouldReturnProblemHttpResultWithError()
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
        _dispatcherMock
            .Setup(dispatcher => dispatcher.DispatchRequestAsync<CreatePostDto, CreatedPostDto>(It.IsAny<CreatePostDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<CreatedPostDto>(ExpectedErrorMessage));

        // Act:
        Results<ProblemHttpResult, CreatedAtRoute<CreatedPostDto>> actualResult = await CreatePostEndpoint.CreatePostAsync(
            _dispatcherMock.Object,
            defaultHttpContext,
            It.IsAny<CreatePostDto>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedProblemHttpResult);
    }

    [Fact]
    public async Task CreatePostAsync_WhenDispatcherReturnsSuccessfulResult_ShouldReturnProblemHttpResultWithError()
    {
        // Arrange:
        var expectedCreatedPostDto = new CreatedPostDto(Guid.NewGuid(), "Title", "Content", "ImagePath");
        var expectedCreatedAtRoute = TypedResults.CreatedAtRoute(
            expectedCreatedPostDto,
            nameof(GetPostByIdEndpoint.GetPostByIdAsync),
            new { expectedCreatedPostDto.Id });
        _dispatcherMock
            .Setup(dispatcher => dispatcher.DispatchRequestAsync<CreatePostDto, CreatedPostDto>(It.IsAny<CreatePostDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedCreatedPostDto));

        // Act:
        Results<ProblemHttpResult, CreatedAtRoute<CreatedPostDto>> actualResult = await CreatePostEndpoint.CreatePostAsync(
            _dispatcherMock.Object,
            It.IsAny<HttpContext>(),
            It.IsAny<CreatePostDto>(),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Result.Should().BeEquivalentTo(expectedCreatedAtRoute);
    }
}
