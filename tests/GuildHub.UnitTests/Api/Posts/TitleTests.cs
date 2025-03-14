namespace GuildHub.UnitTests.Api.Posts;

public sealed class TitleTests
{
    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Build_WhenTitleIsEmpty_ShouldReturnFailureWithErrorMessage(string? title)
    {
        // Act:
        Result<Title> actualTitleResult = Title.Build(title!);

        // Assert:
        actualTitleResult.Should().BeEquivalentTo(Result.Fail("The title cannot be null nor empty."));
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

    [Fact]
    public void BuildNullable_WhenTitleIsNull_ShouldReturnSuccessfulResultWithNullTitle()
    {
        // Act:
        Result<Title?> actualTitleResult = Title.BuildNullable(null);

        // Assert:
        actualTitleResult.IsSuccess.Should().BeTrue();
        actualTitleResult.Value.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void BuildNullable_WhenTrimmedTitleIsEmpty_ShouldReturnFailureWithErrorMessage(string title)
    {
        // Act:
        Result<Title?> actualTitleResult = Title.BuildNullable(title);

        // Assert:
        actualTitleResult.Should().BeEquivalentTo(Result.Fail("The title cannot be empty."));
    }

    [Fact]
    public void BuildNullable_WhenTitleNameIsGreaterThanMaxTitleLength_ShouldReturnFailureWithErrorMessage()
    {
        // Act:
        Result<Title> actualTitleResult = Title.Build(new string('*', PostConstants.MaxTitleLength + 1));

        // Assert:
        actualTitleResult.Should().BeEquivalentTo(Result.Fail($"The title cannot have more than {PostConstants.MaxTitleLength} characters."));
    }

    [Fact]
    public void BuildNullable_WhenTitleIsCorrect_ShouldReturnSuccessWithTheTitleObject()
    {
        // Arrange:
        const string TitleName = "Title";

        // Act:
        Result<Title?> actualTitleResult = Title.BuildNullable(TitleName);

        // Assert:
        actualTitleResult.Value!.ToString().Should().Be(TitleName);
        actualTitleResult.IsSuccess.Should().BeTrue();
    }
}
