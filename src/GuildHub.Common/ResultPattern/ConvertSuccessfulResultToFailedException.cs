namespace GuildHub.Common.ResultPattern;

public sealed class ConvertSuccessfulResultToFailedException : Exception
{
    public ConvertSuccessfulResultToFailedException()
        : base("Cannot convert a successful result to a failed result.")
    {
    }
}
