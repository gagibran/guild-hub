namespace GuildHub.Common.MapHandler;

public sealed class MapDispatcher(IServiceProvider serviceProvider) : IMapDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public TOutput DispatchMap<TInput, TOutput>(TInput input)
        where TInput : class
        where TOutput : class
    {
        var mapper = _serviceProvider.GetRequiredService<IMapHandler<TInput, TOutput>>();
        return mapper.Map(input);
    }
}
