using System.Text.RegularExpressions;

namespace GuildHub.Api.Common.ResultPattern;

public partial class Result
{
    public bool IsSuccess { get; }
    public string ErrorType { get; }
    public string ErrorMessage { get; }

    [GeneratedRegex("\\|+$")]
    private static partial Regex FinalErrorTypeRegex();

    [GeneratedRegex("\\.\\|+$")]
    private static partial Regex FinalErrorMessage();

    protected Result(bool isSuccess, string errorType, string errorMessage)
    {
        if (!isSuccess && (string.IsNullOrWhiteSpace(errorType) || string.IsNullOrWhiteSpace(errorMessage)))
        {
            throw new UnsuccessfulResultMustHaveErrorTypeWithErrorMessageException();
        }
        IsSuccess = isSuccess;
        ErrorType = errorType;
        ErrorMessage = errorMessage;
    }

    public static Result Success()
    {
        return new Result(true, string.Empty, string.Empty);
    }

    public static Result Fail(string errorType, string errorMessage)
    {
        return new Result(false, errorType, errorMessage);
    }

    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new Result<TValue>(true, value, string.Empty, string.Empty);
    }

    public static Result<TValue> Fail<TValue>(string errorType, string errorMessage)
    {
        return new Result<TValue>(false, default, errorType, errorMessage);
    }

    public static Result Combine(params Result[] results)
    {
        var finalErrorType = string.Empty;
        var finalErrorMessage = string.Empty;
        foreach (Result result in results)
        {
            if (!result.IsSuccess)
            {
                finalErrorType += result.ErrorType + "|";
                finalErrorMessage += result.ErrorMessage + "|";
            }
        }
        finalErrorType = FinalErrorTypeRegex().Replace(finalErrorType, "");
        finalErrorMessage = FinalErrorMessage().Replace(finalErrorMessage, ".");
        var isSuccess = string.IsNullOrWhiteSpace(finalErrorMessage);
        return new Result(isSuccess, finalErrorType, finalErrorMessage);
    }
}

public sealed class Result<TValue> : Result
{
    public TValue? Value { get; }

    internal Result(bool isSuccess, TValue? value, string errorType, string errorMessage) : base(isSuccess, errorType, errorMessage)
    {
        Value = value;
    }
}
