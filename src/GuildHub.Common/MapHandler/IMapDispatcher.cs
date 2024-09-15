namespace GuildHub.Common.MapHandler;

public interface IMapDispatcher
{
    TOutput DispatchMapAsync<TInput, TOutput>(TInput input)
        where TInput : class
        where TOutput : class;
}
