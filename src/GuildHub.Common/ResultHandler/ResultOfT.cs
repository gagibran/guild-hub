namespace GuildHub.Common.ResultHandler;

public sealed class Result<TValue> : Result
{
    public TValue? Value { get; }

    private Result(bool isSuccess, TValue? value = default, string? error = null) : base(isSuccess, error)
    {
        Value = value;
    }

    private Result(bool isSuccess, List<string> errors) : base(isSuccess, errors)
    {
    }

    public static Result<TValue> Succeed(TValue value)
    {
        return new Result<TValue>(true, value);
    }

    public static new Result<TValue> Fail(string error)
    {
        return new Result<TValue>(false, error: error);
    }

    public static new Result<TValue> Fail(List<string> errors)
    {
        return new Result<TValue>(false, errors);
    }

    public static Result<TValue> SetTypeToFailedResult(Result failedResult)
    {
        if (failedResult.IsSuccess)
        {
            throw new ConvertSuccessfulResultToFailedResultException();
        }
        return new Result<TValue>(false, failedResult.Errors);
    }
}
