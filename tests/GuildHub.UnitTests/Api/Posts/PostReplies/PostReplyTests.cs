namespace GuildHub.UnitTests.Api.Posts.PostReplies;

public sealed class PostReplyTests
{
    [Fact]
    public void Build_WhenContentResultIsFailure_ShouldReturnFailedResultWithError()
    {
        // Arrange:
        var expectedPostResult = Result<PostReply>.Fail($"The content message cannot have more than {Constants.MaxContentMessageLength} characters.");

        // Act:
        Result<PostReply> actualPostReplyResult = PostReply.Build(
            It.IsAny<Post>(),
            new('*', Constants.MaxContentMessageLength + 1),
            It.IsAny<string?>());

        // Assert:
        actualPostReplyResult.Should().BeEquivalentTo(expectedPostResult);
    }

    [Fact]
    public void Build_WhenContentResultSucceeds_ShouldReturnSuccessfulResultWithPostReply()
    {
        // Act:
        Result<PostReply> actualResult = PostReply.Build(It.IsAny<Post>(), "Content", It.IsAny<string?>());

        // Assert:
        actualResult.IsSuccess.Should().BeTrue();
        actualResult.Value!.Content.Should().Be(Content.Build("Content").Value);
    }
}
