using GuildHub.Api.PostReplies;

namespace GuildHub.UnitTests.Api.Posts;

public class PostTests
{
    [Fact]
    public void Build_WhenTitleResultIsUnsuccessful_ShouldReturnFailureWithErrorMessage()
    {
        // Arrange:
        Result<Post> expectedPostResult = Result<Post>.Fail("The title cannot be empty.");

        // Act:
        Result<Post> actualPostResult = Post.Build("", "Content", "ImagePath");

        // Assert:
        actualPostResult.Should().BeEquivalentTo(expectedPostResult);
    }

    [Fact]
    public void Build_WhenContentResultIsUnsuccessful_ShouldReturnFailureWithErrorMessage()
    {
        // Arrange:
        Result<Post> expectedPostResult = Result<Post>.Fail($"The post content cannot have more than {Constants.MaxContentLength} characters.");

        // Act:
        Result<Post> actualPostResult = Post.Build("Title", new string('*', Constants.MaxContentLength + 1), "ImagePath");

        // Assert:
        actualPostResult.Should().BeEquivalentTo(expectedPostResult);
    }

    [Fact]
    public void Build_WhenTitleResultAndContentResultAreSuccessful_ShouldReturnSuccessfulResultWithPost()
    {
        // Act:
        Result<Post> actualPostResult = Post.Build("Title", "Content", "ImagePath");

        // Assert:
        actualPostResult.IsSuccess.Should().BeTrue();
        actualPostResult.Value!.Title.ToString().Should().Be("Title");
        actualPostResult.Value!.Content!.ToString().Should().Be("Content");
        actualPostResult.Value!.ImagePath.Should().Be("ImagePath");
    }

    [Fact]
    public void Update_WhenCombinedResultIsFailure_ShouldReturnFailure()
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;

        // Act:
        Result actualResult = post.Update(string.Empty, It.IsAny<string?>(), It.IsAny<string?>());

        // Assert:
        actualResult.IsSuccess.Should().BeFalse();
        actualResult.Errors.Should().HaveCount(1);
        actualResult.Errors[0].Should().Be("The title cannot be empty.");
    }

    [Fact]
    public void Update_WhenCombinedResultIsSuccess_ShouldUpdatePost()
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;

        // Act:
        Result actualResult = post.Update("New Title", "New Content", "New ImagePath");

        // Assert:
        actualResult.IsSuccess.Should().BeTrue();
        post.Title.ToString().Should().Be("New Title");
        post.Content!.ToString().Should().Be("New Content");
        post.ImagePath.Should().Be("New ImagePath");
        post.UpdatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 0, 1));
    }

    [Fact]
    public void AddPostReply_WhenReplyIsUnique_ShouldAddReply()
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;
        var postReply = new PostReply(post, "Message", "ImagePath");

        // Act:
        Result actualResult = post.AddPostReply(postReply);

        // Assert:
        actualResult.IsSuccess.Should().BeTrue();
        post.PostReplies.Should().Contain(postReply);
    }

    [Fact]
    public void AddPostReply_ShouldFail_WhenReplyIsDuplicate()
    {
        // Arrange:
        Post post = Post.Build("Title", "Content", "ImagePath").Value!;
        var postReply = new PostReply(post, "Message", "ImagePath");
        post.AddPostReply(postReply);

        // Act:
        Result actualResult = post.AddPostReply(postReply);

        // Assert:
        actualResult.IsSuccess.Should().BeFalse();
        actualResult.Errors.Should().HaveCount(1);
        actualResult.Errors[0].Should().Be($"The reply with ID {postReply.Id} has already been added to this post.");
    }
}
