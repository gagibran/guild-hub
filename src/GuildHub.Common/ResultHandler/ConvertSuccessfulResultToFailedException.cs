namespace GuildHub.Common.ResultHandler;

public sealed class ConvertSuccessfulResultToFailedResultException : Exception
{
    public ConvertSuccessfulResultToFailedResultException()
        : base("Cannot convert a successful result to a failed result.")
    {
    }
}
