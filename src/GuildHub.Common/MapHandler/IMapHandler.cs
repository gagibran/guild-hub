namespace GuildHub.Common.MapHandler;

public interface IMapHandler<TInput, TOutput>
    where TInput : class
    where TOutput : class
{
    TOutput Map(TInput input);
}
