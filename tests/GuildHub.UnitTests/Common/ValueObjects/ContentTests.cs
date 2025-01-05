namespace GuildHub.UnitTests.Common.ValueObjects;

public class ContentTests
{
    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Build_WhenContentIsNullOrWhitespace_ShouldReturnSuccessfulResultWithNullContent(string? content)
    {
        // Act:
        Result<Content?> actualResult = Content.Build(content);

        // Assert:
        actualResult.IsSuccess.Should().BeTrue();
        actualResult.Value.Should().BeNull();
    }

    [Fact]
    public void Build_WhenContentExceedsMaxLength_ShouldFail()
    {
        // Act:
        var actualResult = Content.Build(new string('*', Constants.MaxContentMessageLength + 1));

        // Assert:
        actualResult.IsSuccess.Should().BeFalse();
        actualResult.Errors.Should().Contain($"The content message cannot have more than {Constants.MaxContentMessageLength} characters.");
    }

    [Fact]
    public void Build_WhenContentIsValid_ShouldSucceed()
    {
        // Arrange:
        const string ExpectedContent = "Valid content";

        // Act:
        Result<Content?> actualResult = Content.Build(ExpectedContent);

        // Assert:
        actualResult.IsSuccess.Should().BeTrue();
        actualResult.Value?.Message.Should().Be(ExpectedContent);
    }
}
