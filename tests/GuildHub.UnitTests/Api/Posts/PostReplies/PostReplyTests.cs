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
        Result<PostReply> actualResult = PostReply.Build(
            Post.Build("Title", It.IsAny<string?>(),
            It.IsAny<string?>()).Value!,
            "Content",
            It.IsAny<string?>());

        // Assert:
        actualResult.IsSuccess.Should().BeTrue();
        actualResult.Value!.Content.Should().Be(Content.Build("Content").Value);
    }

    [Fact]
    public void Update_WhenContentAndImagePathAreNull_ShouldReturnFailedResultWithError()
    {
        // Arrange:
        Post post = Post.Build("Title", It.IsAny<string?>(), It.IsAny<string?>()).Value!;
        PostReply postReply = PostReply.Build(post, "Content", It.IsAny<string?>()).Value!;

        // Act:
        Result actualResult = postReply.Update(null, null);

        // Assert:
        actualResult.IsSuccess.Should().BeFalse();
        actualResult.Errors[0].Should().Be("At least one of the following must be provided: content, or imagePath.");
    }

    [Fact]
    public void Update_WhenContentIsEmpty_ShouldReturnFailedResultWithError()
    {
        // Arrange:
        Post post = Post.Build("Title", It.IsAny<string?>(), It.IsAny<string?>()).Value!;
        PostReply postReply = PostReply.Build(post, "Content", It.IsAny<string?>()).Value!;

        // Act:
        Result actualResult = postReply.Update(string.Empty, It.IsAny<string?>());

        // Assert:
        actualResult.IsSuccess.Should().BeFalse();
        actualResult.Errors[0].Should().Be("content must not be empty.");
    }

    [Fact]
    public void Update_WhenContentResultIsFailure_ShouldReturnFailedResultWithError()
    {
        // Arrange:
        Post post = Post.Build("Title", It.IsAny<string?>(), It.IsAny<string?>()).Value!;
        PostReply postReply = PostReply.Build(post, "Content", It.IsAny<string?>()).Value!;

        // Act:
        Result actualResult = postReply.Update(new('*', Constants.MaxContentMessageLength + 1), It.IsAny<string?>());

        // Assert:
        actualResult.IsSuccess.Should().BeFalse();
        actualResult.Errors[0].Should().Be($"The content message cannot have more than {Constants.MaxContentMessageLength} characters.");
    }

    [Theory]
    [InlineData("New Content", "New ImagePath", "New Content", "New ImagePath")]
    [InlineData(null, "New ImagePath", "Content", "New ImagePath")]
    [InlineData("New Content", null, "New Content", "ImagePath")]
    public void Update_WhenParametersAreValid_ShouldUpdateContentAndReturnSuccessfulResult(
        string? content,
        string? imagePath,
        string expectedContent,
        string expectedImagePath)
    {
        // Arrange:
        Post post = Post.Build("Title", It.IsAny<string?>(), It.IsAny<string?>()).Value!;
        PostReply postReply = PostReply.Build(post, "Content", "ImagePath").Value!;

        // Act:
        Result actualResult = postReply.Update(content, imagePath);

        // Assert:
        actualResult.IsSuccess.Should().BeTrue();
        postReply.Content.Message.Should().Be(expectedContent);
        postReply.ImagePath.Should().Be(expectedImagePath);
    }
}
