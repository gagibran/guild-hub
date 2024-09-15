using GuildHub.Common.MapHandler;
using GuildHub.Common.RequestHandler;

namespace GuildHub.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddCommonServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IRequestDispatcher, RequestDispatcher>();
        serviceCollection.AddTransient<IMapDispatcher, MapDispatcher>();
        return serviceCollection;
    }
}
