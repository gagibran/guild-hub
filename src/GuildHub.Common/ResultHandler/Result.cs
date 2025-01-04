namespace GuildHub.Common.ResultHandler;

public class Result
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
        Errors = isSuccess ? [] : [error];
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

    public static Result Succeed()
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

    public static Result Combine(Result result1, Result result2)
    {
        if (result1.IsSuccess && result2.IsSuccess)
        {
            return new Result(true);
        }
        if (!result1.IsSuccess && !result2.IsSuccess)
        {
            return new Result(false, [.. result1.Errors, .. result2.Errors]);
        }
        if (!result1.IsSuccess)
        {
            return new Result(false, result1.Errors);
        }
        return new Result(false, result2.Errors);
    }
}
