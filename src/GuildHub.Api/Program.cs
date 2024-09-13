using GuildHub.Api.Common;
using GuildHub.Api.Data;

WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);
webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(dbContextOptionsBuilder =>
{
    dbContextOptionsBuilder.UseNpgsql(webApplicationBuilder.Configuration.GetConnectionString("Application"));
});
webApplicationBuilder.Services.AddScoped<IApplicationDbContext>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
webApplicationBuilder.Services.AddPostServices();
webApplicationBuilder.Services.AddCommonServices();

WebApplication webApplication = webApplicationBuilder.Build();
webApplication.UseHttpsRedirection();
webApplication.AddPostEndpoints();
webApplication.Run();
