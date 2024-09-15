WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);
webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(dbContextOptionsBuilder =>
{
    dbContextOptionsBuilder.UseNpgsql(webApplicationBuilder.Configuration.GetConnectionString("Application"));
});
webApplicationBuilder.Services.AddScoped<IApplicationDbContext>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
webApplicationBuilder.Services.AddExceptionHandler<GlobalExceptionHandler>();
webApplicationBuilder.Services.AddCommonServices();
webApplicationBuilder.Services.AddPostServices();

WebApplication webApplication = webApplicationBuilder.Build();
webApplication.UseExceptionHandler(_ => {});
webApplication.UseHttpsRedirection();
webApplication.AddPostEndpoints();
webApplication.Run();
