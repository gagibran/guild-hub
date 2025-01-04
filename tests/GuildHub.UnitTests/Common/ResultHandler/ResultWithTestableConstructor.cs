namespace GuildHub.UnitTests.Common.ResultHandler;

internal sealed class ResultWithTestableConstructor : Result
{
    public ResultWithTestableConstructor(bool isSuccess, string? error = null) : base(isSuccess, error)
    {
    }

    public ResultWithTestableConstructor(bool isSuccess, List<string> errors) : base(isSuccess, errors)
    {
    }
}
