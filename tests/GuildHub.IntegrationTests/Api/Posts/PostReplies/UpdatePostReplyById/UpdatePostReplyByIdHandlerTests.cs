using GuildHub.Api.Posts.PostReplies.UpdatePostReplyById;

namespace GuildHub.IntegrationTests.Api.Posts.PostReplies.UpdatePostReplyById;

[Collection(nameof(SharedDatabaseFixture))]
public sealed class UpdatePostReplyByIdHandlerTests : IntegrationTest
{
    private readonly IRequestHandler<UpdatePostReplyByIdRequest> _updatePostReplyByIdHandler;

    public UpdatePostReplyByIdHandlerTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
        : base(integrationTestsWebApplicationFactory)
    {
        _updatePostReplyByIdHandler = ServiceProvider.GetRequiredService<IRequestHandler<UpdatePostReplyByIdRequest>>();
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostIsNull_ShouldReturnFailedResult()
    {
        // Arrange:
        var updatePostReplyByIdRequest = new UpdatePostReplyByIdRequest(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<UpdatePostReplyByIdDto>());
        var expectedResult = Result.Fail($"No post with the ID '{updatePostReplyByIdRequest.PostId}' was found.");

        // Act:
        Result actualResult = await _updatePostReplyByIdHandler.HandleAsync(updatePostReplyByIdRequest, It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostReplyIsNull_ShouldReturnFailedResult()
    {
        // Arrange:
        Post newPost = Post.Build("Title", "Content", "ImagePath").Value!;
        var updatePostReplyByIdRequest = new UpdatePostReplyByIdRequest(newPost.Id, It.IsAny<Guid>(), It.IsAny<UpdatePostReplyByIdDto>());
        var expectedResult = Result.Fail($"No post reply with the ID '{updatePostReplyByIdRequest.Id}' was found.");
        ApplicationDbContext.Posts.Add(newPost);
        await ApplicationDbContext.SaveChangesAsync();

        // Act:
        Result actualResult = await _updatePostReplyByIdHandler.HandleAsync(updatePostReplyByIdRequest, It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenUpdatePostReplyResultIsNotSuccessful_ShouldReturnFailedResult()
    {
        // Arrange:
        Post newPost = Post.Build("Title", "Content", "ImagePath").Value!;
        PostReply newPostReply = PostReply.Build(newPost, "Content", "ImagePath").Value!;
        var updatePostReplyByIdRequest = new UpdatePostReplyByIdRequest(newPost.Id, newPostReply.Id, new(string.Empty, "NewImagePath"));
        var expectedResult = Result.Fail("content must not be empty.");
        ApplicationDbContext.Posts.Add(newPost);
        ApplicationDbContext.PostReplies.Add(newPostReply);
        await ApplicationDbContext.SaveChangesAsync();

        // Act:
        Result actualResult = await _updatePostReplyByIdHandler.HandleAsync(updatePostReplyByIdRequest, It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Theory]
    [InlineData("NewContent", "NewImagePath")]
    [InlineData("NewContent", null)]
    [InlineData(null, "NewImagePath")]
    public async Task HandleAsync_WhenUpdatePostReplyResultIsSuccessful_ShouldReturnSuccessfulResult(string? newContent, string? newImagePath)
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;
        PostReply postReply = PostReply.Build(post, "Content", "ImagePath").Value!;
        var updatePostReplyByIdRequest = new UpdatePostReplyByIdRequest(post.Id, postReply.Id, new(newContent, newImagePath));
        ApplicationDbContext.Posts.Add(post);
        ApplicationDbContext.PostReplies.Add(postReply);
        await ApplicationDbContext.SaveChangesAsync();

        // Act:
        Result actualResult = await _updatePostReplyByIdHandler.HandleAsync(updatePostReplyByIdRequest, It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(Result.Succeed());
        PostReply actualRetrievedPostReply = (await ApplicationDbContext.PostReplies.FindAsync(postReply.Id))!;
        actualRetrievedPostReply.Should().BeEquivalentTo(
            postReply,
            options => options
                .Using<DateTime>(assertionContext => assertionContext.Subject
                    .Should()
                    .BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(1)))
                .When(dto => dto.Path.EndsWith("CreatedAtUtc") || dto.Path.EndsWith("UpdatedAtUtc")));
    }
}
