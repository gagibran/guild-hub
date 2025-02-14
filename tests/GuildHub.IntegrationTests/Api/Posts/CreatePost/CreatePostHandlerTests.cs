using Microsoft.EntityFrameworkCore;

namespace GuildHub.IntegrationTests.Api.Posts.CreatePost;

[Collection(nameof(SharedDatabaseFixture))]
public sealed class CreatePostHandlerTests : IntegrationTest
{
    private readonly IRequestHandler<CreatePostDto, CreatedPostDto> _createPostHandler;

    public CreatePostHandlerTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
        : base(integrationTestsWebApplicationFactory)
    {
        _createPostHandler = ServiceProvider.GetRequiredService<IRequestHandler<CreatePostDto, CreatedPostDto>>();
    }

    [Fact]
    public async Task HandleAsync_WhenPostResultFails_ShouldReturnFailedResult()
    {
        // Arrange:
        Result<CreatedPostDto> expectedResult = Result<CreatedPostDto>.Fail("The title cannot be empty.");

        // Act:
        Result<CreatedPostDto> actualResult = await _createPostHandler.HandleAsync(
            new CreatePostDto(string.Empty, It.IsAny<string>(), It.IsAny<string>()),
            It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenPostResultSucceeds_ShouldReturnSuccessfulResultWithDto()
    {
        // Act:
        Result<CreatedPostDto> actualResult = await _createPostHandler.HandleAsync(
            new CreatePostDto("Title", "Content", "ImagePath"),
            It.IsAny<CancellationToken>());

        // Assert:
        Result<CreatedPostDto> expectedResult = await ApplicationDbContext.Posts
            .Where(post => post.Id == actualResult.Value!.Id)
            .Select(post => Result<CreatedPostDto>.Succeed(new(
                post.Id,
                post.Title.ToString(),
                post.Content!.ToString(),
                post.ImagePath)))
            .SingleAsync();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}
