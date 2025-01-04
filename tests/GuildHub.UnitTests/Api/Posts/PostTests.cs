using GuildHub.Api.PostReplies;

namespace GuildHub.UnitTests.Api.Posts;

public class PostTests
{
    [Fact]
    public void AddPostReply_WhenReplyIsUnique_ShouldAddReply()
    {
        // Arrange:
        var post = new Post(Title.Build("Test Title").Value!, Content.Build("Test Content").Value!, "ImagePath");
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
        var post = new Post(Title.Build("Test Title").Value!, Content.Build("Test Content").Value!, "ImagePath");
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
