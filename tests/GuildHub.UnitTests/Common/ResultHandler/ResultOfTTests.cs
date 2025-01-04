namespace GuildHub.UnitTests.Common.ResultHandler;

public class Result_1Tests
{

    [Fact]
    public void SetTypeToFailedResult_WhenResultPassedIsSuccessful_ShouldThrowConvertSuccessfulResultToFailedResultException()
    {
        // Arrange:
        Result result1 = Result.Succeed();

        // Act:
        Action Act = () => Result<int>.SetTypeToFailedResult(result1);

        // Assert:
        Act
            .Should()
            .Throw<ConvertSuccessfulResultToFailedResultException>()
            .WithMessage("Cannot convert a successful result to a failed result.");
    }

    [Fact]
    public void SetTypeToFailedResult_WhenResultPassedIsFailure_ShouldReturnFailedResultWithTypeAndErrorMessage()
    {
        // Arrange:
        Result result1 = Result.Fail("Error");

        // Act:
        Result<int> actualIntResult = Result<int>.SetTypeToFailedResult(result1);

        // Assert:
        actualIntResult.IsSuccess.Should().BeFalse();
        actualIntResult.Errors.Should().HaveCount(1);
        actualIntResult.Errors.Should().Contain("Error");
        actualIntResult.Should().BeOfType<Result<int>>();
    }
}
