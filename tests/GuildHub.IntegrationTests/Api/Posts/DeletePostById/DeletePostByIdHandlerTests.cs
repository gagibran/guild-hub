using GuildHub.Api.Posts.DeletePostById;

namespace GuildHub.IntegrationTests.Api.Posts.DeletePostById;

[Collection(nameof(SharedDatabaseFixture))]
public sealed class DeletePostByIdHandlerTests : IntegrationTest
{
    private readonly IRequestHandler<DeletePostByIdRequest> _deletePostByIdHandler;
    public DeletePostByIdHandlerTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
        : base(integrationTestsWebApplicationFactory)
    {
        _deletePostByIdHandler = ServiceProvider.GetRequiredService<IRequestHandler<DeletePostByIdRequest>>();
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostIsNull_ShouldReturnFailedResultWithErrorMessage()
    {
        // Arrange:
        Guid invalidId = Guid.NewGuid();
        Result expectedResult = Result.Fail($"No post with the ID '{invalidId}' was found.");

        // Act:
        Result actualResult = await _deletePostByIdHandler.HandleAsync(new(invalidId), It.IsAny<CancellationToken>());

        // Assert:
        expectedResult.Should().BeEquivalentTo(actualResult);
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostIsNotNull_ShouldReturnSuccessfulResult()
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;
        await ApplicationDbContext.Posts.AddAsync(post);
        await ApplicationDbContext.SaveChangesAsync();

        // Act:
        Result actualResult = await _deletePostByIdHandler.HandleAsync(new(post.Id), It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(Result.Succeed());
    }
}
