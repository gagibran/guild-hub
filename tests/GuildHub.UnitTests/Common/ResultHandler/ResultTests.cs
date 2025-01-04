namespace GuildHub.UnitTests.Common.ResultHandler;

public class ResultTests
{
    public static TheoryData<Result, Result> CombineWhenSuccessfulAndFailedResultShouldReturnFailedResultWithErrorsTestData()
    {
        return new TheoryData<Result, Result>
        {
            { Result.Succeed(), Result.Fail("Error") },
            { Result.Fail("Error"), Result.Succeed() }
        };
    }

    public static TheoryData<List<string?>?> ResultWhenIsSuccessIsTrueAndErrorsIsNullOrEmptyOrHaveAnyInvalidErrorMessageShouldThrowUnsuccessfulResultMustHaveErrorTypeWithErrorMessageExceptionTestData()
    {
        return
        [
            null,
            [],
            [string.Empty],
            [null]
        ];
    }

    [Fact]
    public void Result_WhenFailedResultAndInvalidErrorMessage_ShouldThrowUnsuccessfulResultMustHaveErrorTypeWithErrorMessageException()
    {
        // Act:
        Action act = static () => new ResultWithTestableConstructor(false, string.Empty);

        // Assert:
        act
            .Should()
            .Throw<UnsuccessfulResultMustHaveErrorTypeWithErrorMessageException>()
            .WithMessage("Unsuccessful result must have an error type with an error message.");
    }

    [Fact]
    public void Result_WhenIsSuccessIsTrue_ShouldHaveErrorsAsEmptyList()
    {
        // Act:
        Result actualResult = new ResultWithTestableConstructor(true);

        // Assert:
        actualResult.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Result_WhenIsSuccessIsFalse_ShouldHaveErrorMessageInErrors()
    {
        // Act:
        Result actualResult = new ResultWithTestableConstructor(false, "Error");

        // Assert:
        actualResult.Errors.Should().HaveCount(1);
        actualResult.Errors.Should().Contain("Error");
    }

    [Fact]
    public void Result_WhenIsSuccessIsFalseAndErrorsIsNotNullNorEmptyNorHaveAnyInvalidErrorMessage_ShouldHaveErrorsAsProvidedErrors()
    {
        // Arrange:
        var errors = new List<string> { "Error 1", "Error 2" };

        // Act:
        Result actualResult = new ResultWithTestableConstructor(false, errors);

        // Assert:
        actualResult.Errors.Should().HaveCount(2);
        actualResult.Errors.Should().Contain("Error 1");
        actualResult.Errors.Should().Contain("Error 2");
    }

    [Theory]
    [MemberData(nameof(ResultWhenIsSuccessIsTrueAndErrorsIsNullOrEmptyOrHaveAnyInvalidErrorMessageShouldThrowUnsuccessfulResultMustHaveErrorTypeWithErrorMessageExceptionTestData))]
    public void Result_WhenIsSuccessIsTrueAndErrorsIsNullOrEmptyOrHaveAnyInvalidErrorMessage_ShouldThrowUnsuccessfulResultMustHaveErrorTypeWithErrorMessageException(
        List<string?>? errors)
    {
        // Act:
        Action act = () => new ResultWithTestableConstructor(false, errors!);

        // Assert:
        act
            .Should()
            .Throw<UnsuccessfulResultMustHaveErrorTypeWithErrorMessageException>()
            .WithMessage("Unsuccessful result must have an error type with an error message.");
    }

    [Fact]
    public void Combine_WhenTwoResultsAreSuccessful_ShouldReturnSuccessfulResult()
    {
        // Arrange:
        Result result1 = Result.Succeed();
        Result result2 = Result.Succeed();

        // Act:
        Result actualCombinedResult = Result.Combine(result1, result2);

        // Assert:
        actualCombinedResult.IsSuccess.Should().BeTrue();
        actualCombinedResult.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Combine_WhenTwoResultsFail_ShouldReturnFailedResultWithCombinedErrors()
    {
        // Arrange:
        Result result1 = Result.Fail("Error 1");
        Result result2 = Result.Fail("Error 2");

        // Act:
        Result actualCombinedResult = Result.Combine(result1, result2);

        // Assert:
        actualCombinedResult.IsSuccess.Should().BeFalse();
        actualCombinedResult.Errors.Should().HaveCount(2);
        actualCombinedResult.Errors.Should().Contain("Error 1");
        actualCombinedResult.Errors.Should().Contain("Error 2");
    }

    [Theory]
    [MemberData(nameof(CombineWhenSuccessfulAndFailedResultShouldReturnFailedResultWithErrorsTestData))]
    public void Combine_WhenSuccessfulAndFailedResult_ShouldReturnFailedResultWithErrors(Result result1, Result result2)
    {
        // Act:
        Result actualCombinedResult = Result.Combine(result1, result2);

        // Assert:
        actualCombinedResult.IsSuccess.Should().BeFalse();
        actualCombinedResult.Errors.Should().HaveCount(1);
        actualCombinedResult.Errors.Should().Contain("Error");
    }
}
