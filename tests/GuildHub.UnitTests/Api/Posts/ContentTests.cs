namespace GuildHub.UnitTests.Api.Posts;

public sealed class ContentTests
{
    public static TheoryData<string, string> BuildNullableWhenTrimmedContentIsValidShouldSuccessfulResultWithContentTestData()
    {
        return
        new()
        {
            { "Content", "Content" },
            {
                $"{new(' ', Constants.MaxContentMessageLength / 2)}Content{new(' ', Constants.MaxContentMessageLength / 2)}",
                "Content"
            }
        };
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void BuildNullable_WhenContentIsNullOrWhitespace_ShouldReturnSuccessfulResultWithNullContent(string? content)
    {
        // Act:
        Result<Content?> actualResult = Content.BuildNullable(content);

        // Assert:
        actualResult.IsSuccess.Should().BeTrue();
        actualResult.Value.Should().BeNull();
    }

    [Fact]
    public void BuildNullable_WhenTrimmedContentIsGreaterThanMaxCharacters_ShouldReturnFailureWithErrorMessage()
    {
        // Arrange:
        Result<Content?> expectedResult = Result<Content?>.Fail($"The content message cannot have more than {Constants.MaxContentMessageLength} characters.");

        // Act:
        Result<Content?> actualResult = Content.BuildNullable(new('*', Constants.MaxContentMessageLength + 1));

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Theory]
    [MemberData(nameof(BuildNullableWhenTrimmedContentIsValidShouldSuccessfulResultWithContentTestData))]
    public void BuildNullable_WhenTrimmedContentIsValid_ShouldSuccessfulResultWithContent(string content, string expectedReturnedContent)
    {
        // Act:
        Result<Content?> actualResult = Content.BuildNullable(content);

        // Assert:
        actualResult.IsSuccess.Should().BeTrue();
        actualResult.Value!.ToString().Should().Be(expectedReturnedContent);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Build_WhenContentIsNullOrWhitespace_ShouldReturnFailedResultWithErrorMessage(string? content)
    {
        // Arrange:
        Result<Content> expectedResult = Result<Content>.Fail($"The content message cannot be null nor empty.");

        // Act:
        Result<Content> actualResult = Content.Build(content!);

        // Assert:
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}
