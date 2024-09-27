namespace GuildHub.Api;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);
        AddServices(webApplicationBuilder);
        WebApplication webApplication = webApplicationBuilder.Build();
        ConfigureHttpRequestPipeline(webApplication);
        webApplication.Run();
    }

    private static void AddServices(WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(dbContextOptionsBuilder =>
        {
            dbContextOptionsBuilder.UseNpgsql(webApplicationBuilder.Configuration.GetConnectionString("Application"));
        });
        webApplicationBuilder.Services.AddScoped<IApplicationDbContext>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
        webApplicationBuilder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        webApplicationBuilder.Services.AddCommonServices();
        webApplicationBuilder.Services.AddPostServices();
    }

    private static void ConfigureHttpRequestPipeline(WebApplication webApplication)
    {
        webApplication.UseExceptionHandler(_ => { });
        webApplication.UseHttpsRedirection();
        webApplication.AddPostEndpoints();
    }
}
