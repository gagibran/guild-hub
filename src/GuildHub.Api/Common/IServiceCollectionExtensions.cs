using GuildHub.Api.Common.DispatcherPattern;

namespace GuildHub.Api.Common;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddCommonServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDispatcher, Dispatcher>();
        return serviceCollection;
    }
}
