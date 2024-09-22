namespace GuildHub.Common.MapHandler;

public interface IMapDispatcher
{
    TOutput DispatchMap<TInput, TOutput>(TInput input)
        where TInput : class
        where TOutput : class;
}
