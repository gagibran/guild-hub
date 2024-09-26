namespace GuildHub.Common.ResultHandler;

public partial class Result
{
    public bool IsSuccess { get; }
    public List<string> Errors { get; }

    protected Result(bool isSuccess, string? error = null)
    {
        if (!isSuccess && string.IsNullOrWhiteSpace(error))
        {
            throw new UnsuccessfulResultMustHaveErrorTypeWithErrorMessageException();
        }
        IsSuccess = isSuccess;
        Errors = [error];
    }

    protected Result(bool isSuccess, List<string> errors)
    {
        if (!isSuccess && (errors is null || errors.Count is 0 || errors.Any(string.IsNullOrWhiteSpace)))
        {
            throw new UnsuccessfulResultMustHaveErrorTypeWithErrorMessageException();
        }
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success()
    {
        return new Result(true);
    }

    public static Result Fail(string error)
    {
        return new Result(false, error);
    }

    public static Result Fail(List<string> errors)
    {
        return new Result(false, errors);
    }

    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new Result<TValue>(true, value);
    }

    public static Result<TValue> Fail<TValue>(string error)
    {
        return new Result<TValue>(false, error: error);
    }

    public static Result<TValue> Fail<TValue>(List<string> errors)
    {
        return new Result<TValue>(false, errors);
    }

    public static Result<TValue> Fail<TValue>(Result failedResult)
    {
        if (failedResult.IsSuccess)
        {
            throw new ConvertSuccessfulResultToFailedException();
        }
        return new Result<TValue>(false, failedResult.Errors);
    }
}

public sealed class Result<TValue> : Result
{
    public TValue? Value { get; }

    internal Result(bool isSuccess, TValue? value = default, string? error = null) : base(isSuccess, error)
    {
        Value = value;
    }

    internal Result(bool isSuccess, List<string> errors) : base(isSuccess, errors)
    {
    }
}
