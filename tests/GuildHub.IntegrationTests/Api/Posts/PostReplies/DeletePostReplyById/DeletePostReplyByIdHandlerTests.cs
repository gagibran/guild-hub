using GuildHub.Api.Posts.PostReplies.DeletePostReplyById;

namespace GuildHub.IntegrationTests.Api.Posts.PostReplies.DeletePostReplyById;

[Collection(nameof(SharedDatabaseFixture))]
public sealed class DeletePostReplyByIdHandlerTests : IntegrationTest
{
    private readonly IRequestHandler<DeletePostReplyByIdRequest> _deletePostReplyByIdHandler;

    public DeletePostReplyByIdHandlerTests(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
        : base(integrationTestsWebApplicationFactory)
    {
        _deletePostReplyByIdHandler = ServiceProvider.GetRequiredService<IRequestHandler<DeletePostReplyByIdRequest>>();
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostIsNull_ShouldReturnFailedResult()
    {
        // Arrange:
        var deletePostReplyByIdRequest = new DeletePostReplyByIdRequest(It.IsAny<Guid>(), It.IsAny<Guid>());
        var expectedResult = Result.Fail($"No post with the ID '{deletePostReplyByIdRequest.PostId}' was found.");

        // Act:
        Result actualResult = await _deletePostReplyByIdHandler.HandleAsync(deletePostReplyByIdRequest, It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenRetrievedPostReplyIsNull_ShouldReturnFailedResult()
    {
        // Arrange:
        Post newPost = Post.Build("Title", "Content", "ImagePath").Value!;
        var deletePostReplyByIdRequest = new DeletePostReplyByIdRequest(newPost.Id, It.IsAny<Guid>());
        var expectedResult = Result.Fail($"No post reply with the ID '{deletePostReplyByIdRequest.Id}' was found.");
        ApplicationDbContext.Posts.Add(newPost);
        await ApplicationDbContext.SaveChangesAsync();

        // Act:
        Result actualResult = await _deletePostReplyByIdHandler.HandleAsync(deletePostReplyByIdRequest, It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_WhenDeletePostReplyResultIsSuccessful_ShouldReturnSucceedResult()
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;
        PostReply postReply = PostReply.Build(post, "Content", "ImagePath").Value!;
        var deletePostReplyByIdRequest = new DeletePostReplyByIdRequest(post.Id, postReply.Id);
        ApplicationDbContext.Posts.Add(post);
        ApplicationDbContext.PostReplies.Add(postReply);
        await ApplicationDbContext.SaveChangesAsync();

        // Act:
        Result actualResult = await _deletePostReplyByIdHandler.HandleAsync(deletePostReplyByIdRequest, It.IsAny<CancellationToken>());

        // Assert:
        actualResult.Should().BeEquivalentTo(Result.Succeed());
        ApplicationDbContext.PostReplies.Should().NotContain(postReply);
    }
}
