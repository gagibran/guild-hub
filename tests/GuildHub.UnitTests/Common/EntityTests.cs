using GuildHub.Api.PostReplies;

namespace GuildHub.UnitTests.Common;

public sealed class EntityTests
{
    public static TheoryData<Post?, PostReply?> EqualsOperatorWhenAtLeastOneEntityIsNotNullShouldReturnFalseData()
    {
        return new()
        {
            { new(It.IsAny<Title>(), It.IsAny<string>(), It.IsAny<string>()), null },
            { null, new(It.IsAny<Post>(), It.IsAny<string>(), It.IsAny<string>()) },
        };
    }

    [Fact]
    public void Equals_WhenObjIsNull_ShouldReturnFalse()
    {
        // Arrange:
        var post = new Post(It.IsAny<Title>(), It.IsAny<string>(), It.IsAny<string>());

        // Act:
        bool actualEquals = post.Equals(null);

        // Assert:
        actualEquals.Should().BeFalse();
    }

    [Fact]
    public void Equals_WhenObjTypeIsDifferentFromEntity_ShouldReturnFalse()
    {
        // Arrange:
        var post = new Post(It.IsAny<Title>(), It.IsAny<string>(), It.IsAny<string>());
        var postReply = new PostReply(It.IsAny<Post>(), It.IsAny<string>(), It.IsAny<string>());

        // Act:
        bool actualEquals = post.Equals(postReply);

        // Assert:
        actualEquals.Should().BeFalse();
    }

    [Fact]
    public void Equals_WhenIdsAreEquals_ShouldReturnTrue()
    {
        // Arrange:
        var post1 = new Post(It.IsAny<Title>(), It.IsAny<string>(), It.IsAny<string>());
        var post2 = post1;

        // Act:
        bool actualEquals = post1.Equals(post2);

        // Assert:
        actualEquals.Should().BeTrue();
    }

    [Fact]
    public void EqualsOperator_WhenTheTwoEntitiesAreNull_ShouldReturnTrue()
    {
        // Arrange:
        Post? post = null;
        PostReply? postReply = null;

        // Act:
        bool actualEquals = post! == postReply!;

        // Assert:
        actualEquals.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(EqualsOperatorWhenAtLeastOneEntityIsNotNullShouldReturnFalseData))]
    public void EqualsOperator_WhenAtLeastOneEntityIsNotNull_ShouldReturnFalse(Post? post, PostReply? postReply)
    {
        // Act:
        bool actualEquals = post! == postReply!;

        // Assert:
        actualEquals.Should().BeFalse();
    }

    [Fact]
    public void EqualsOperator_WhenIdsAreEquals_ShouldReturnTrue()
    {
        // Arrange:
        var postReply1 = new PostReply(It.IsAny<Post>(), It.IsAny<string>(), It.IsAny<string>());
        var postReply2 = postReply1;

        // Act:
        bool actualEquals = postReply1 == postReply2;

        // Assert:
        actualEquals.Should().BeTrue();
    }
}
