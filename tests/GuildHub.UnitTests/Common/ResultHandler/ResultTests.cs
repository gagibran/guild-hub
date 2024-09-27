namespace GuildHub.UnitTests.Common.ResultHandler;

public sealed class ResultTests
{
    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Fail_WhenErrorIsNullOrWhiteSpace_ShouldThrowUnsuccessfulResultMustHaveErrorTypeWithErrorMessageException(string error)
    {
        // Arrange:
        Func<Result> act = () => Result.Fail(error);

        // Act & Assert:
        act
            .Should()
            .Throw<UnsuccessfulResultMustHaveErrorTypeWithErrorMessageException>()
            .WithMessage("An unsuccessful result must have an error type with an error message.");
    }

    [Fact]
    public void Fail_WhenErrorsIsNull_ShouldThrowUnsuccessfulResultMustHaveErrorTypeWithErrorMessageException()
    {
        // Arrange:
        Func<Result> act = () => Result.Fail((List<string>)null!);

        // Act & Assert:
        act
            .Should()
            .Throw<UnsuccessfulResultMustHaveErrorTypeWithErrorMessageException>()
            .WithMessage("An unsuccessful result must have an error type with an error message.");
    }

    [Fact]
    public void Fail_WhenErrorsIsEmpty_ShouldThrowUnsuccessfulResultMustHaveErrorTypeWithErrorMessageException()
    {
        // Arrange:
        Func<Result> act = () => Result.Fail([]);

        // Act & Assert:
        act
            .Should()
            .Throw<UnsuccessfulResultMustHaveErrorTypeWithErrorMessageException>()
            .WithMessage("An unsuccessful result must have an error type with an error message.");
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Fail_WhenErrorsContainsNullOrWhiteSpaceStrings_ShouldThrowUnsuccessfulResultMustHaveErrorTypeWithErrorMessageException(string error)
    {
        // Arrange:
        Func<Result> act = () => Result.Fail(["Error", error, "Error2"]);

        // Act & Assert:
        act
            .Should()
            .Throw<UnsuccessfulResultMustHaveErrorTypeWithErrorMessageException>()
            .WithMessage("An unsuccessful result must have an error type with an error message.");
    }

    [Fact]
    public void Fail_WhenFailedResultIsSuccessPropertyIsTrue_ShouldThrowConvertSuccessfulResultToFailedException()
    {
        // Arrange:
        Result successfulResult = Result.Success();
        Func<Result> act = () => Result.Fail<string>(successfulResult);

        // Act & Assert:
        act
            .Should()
            .Throw<ConvertSuccessfulResultToFailedException>()
            .WithMessage("Cannot convert a successful result to a failed result.");
    }

    [Fact]
    public void Fail_WhenFailedResultIsSuccessPropertyIsFalse_ShouldReturnFailedResult()
    {
        // Arrange:
        const string ExpectedErrorMessage = "Failed error";
        Result<string> failedResult = Result.Fail<string>(ExpectedErrorMessage);

        // Act:
        Result<int> actualFailedResult = Result.Fail<int>(failedResult);

        // Assert:
        actualFailedResult.Should().BeEquivalentTo(Result.Fail<int>(ExpectedErrorMessage));
    }
}
