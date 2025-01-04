namespace GuildHub.UnitTests.Api.Posts;

public sealed class TitleTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Build_WhenTitleIsEmpty_ShouldReturnFailureWithErrorMessage(string? title)
    {
        // Act:
        Result<Title> actualTitleResult = Title.Build(title!);

        // Assert:
        actualTitleResult.Should().BeEquivalentTo(Result.Fail("The post title cannot be empty."));
    }

    [Fact]
    public void Build_WhenTitleNameIsGreaterThanMaxTitleLength_ShouldReturnFailureWithErrorMessage()
    {
        // Act:
        Result<Title> actualTitleResult = Title.Build(new string('*', PostConstants.MaxTitleLength + 1));

        // Assert:
        actualTitleResult.Should().BeEquivalentTo(Result.Fail($"The post title cannot have more than {PostConstants.MaxTitleLength} characters."));
    }

    [Fact]
    public void Build_WhenTitleIsCorrect_ShouldReturnSuccessWithTheTitleObject()
    {
        // Arrange:
        const string TitleName = "Title";

        // Act:
        Result<Title> actualTitleResult = Title.Build(TitleName);

        // Assert:
        actualTitleResult.Value!.ToString().Should().Be(TitleName);
        actualTitleResult.IsSuccess.Should().BeTrue();
    }
}
