using GuildHub.Api.Posts;
using GuildHub.Api.Posts.ValueObjects;

namespace GuildHub.UnitTests.Posts.ValueObjects;

public class TitleTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Build_WhenTitleIsEmpty_ShouldReturnFailureWithErrorMessage(string title)
    {
        // Act:
        Result<Title> actualTitleResult = Title.Build(title);

        // Assert:
        actualTitleResult.Should().BeEquivalentTo(Result.Fail("The post title cannot be empty."));
    }

    [Fact]
    public void Build_WhenTitleNameIsGreaterThan300Characters_ShouldReturnFailureWithErrorMessage()
    {
        // Act:
        Result<Title> actualTitleResult = Title.Build("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec p");

        // Assert:
        actualTitleResult.Should().BeEquivalentTo(Result.Fail($"The post title cannot have a more than {Constants.MaxTitleLength}."));
    }

    [Fact]
    public void Build_WhenTitleIsCorrect_ShouldReturnSuccessWithTheTitleObject()
    {
        // Arrange

        // Act:
        Result<Title> actualTitleResult = Title.Build("Correct Title");

        // Assert:
        actualTitleResult.Value!.ToString().Should().Be("Correct Title");
        actualTitleResult.IsSuccess.Should().BeTrue();
    }
}
