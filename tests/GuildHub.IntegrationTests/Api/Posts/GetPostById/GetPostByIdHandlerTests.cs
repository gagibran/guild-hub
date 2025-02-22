namespace GuildHub.IntegrationTests.Api.Posts.GetPostById;

[Collection(nameof(SharedDatabaseFixture))]
public sealed class GetPostByIdHandlerTests : IntegrationTest
{
    private readonly IRequestHandler<GetPostByIdRequest, RetrievedPostByIdDto> _getPostByIdHandler;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetPostByIdHandlerTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory) : base(integrationTestsWebApplicationFactory)
    {
        _getPostByIdHandler = ServiceProvider.GetRequiredService<IRequestHandler<GetPostByIdRequest, RetrievedPostByIdDto>>();
        _httpContextAccessor = ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "http";
        httpContext.Request.Host = new HostString("localhost");
        _httpContextAccessor.HttpContext = httpContext;
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostIsNull_ShouldReturnFailedResultWithErrorMessage()
    {
        // Arrange:
        Guid invalidId = Guid.NewGuid();
        Result<RetrievedPostByIdDto> expectedResult = Result<RetrievedPostByIdDto>.Fail($"No post with the ID '{invalidId}' was found.");

        // Act:
        Result<RetrievedPostByIdDto> actualResult = await _getPostByIdHandler.HandleAsync(new(invalidId), It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostIsNotNull_ShouldReturnSuccessfulResult()
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;
        await ApplicationDbContext.Posts.AddAsync(post);
        await ApplicationDbContext.SaveChangesAsync();
        Result<RetrievedPostByIdDto> expectedResult = Result<RetrievedPostByIdDto>.Succeed(new(
            post.Id,
            post.Title.ToString(),
            post.Content!.ToString(),
            post.ImagePath,
            $"http://localhost/api/posts/{post.Id}/replies",
            post.CreatedAtUtc,
            post.UpdatedAtUtc));

        // Act:
        Result<RetrievedPostByIdDto> actualResult = await _getPostByIdHandler.HandleAsync(new(post.Id), It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}
