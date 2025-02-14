using GuildHub.Api.Data;
using GuildHub.Common.MapHandler;
using Microsoft.EntityFrameworkCore;

namespace GuildHub.IntegrationTests;

[Collection(nameof(SharedDatabaseFixture))]
public abstract class IntegrationTest : IAsyncLifetime
{
    private readonly IServiceScope _serviceScope;

    protected HttpClient HttpClient { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected ApplicationDbContext ApplicationDbContext { get; }
    protected JsonSerializerOptions JsonSerializerOptions { get; }

    protected IntegrationTest(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
    {
        _serviceScope = integrationTestsWebApplicationFactory.Services.CreateScope();
        ServiceProvider = _serviceScope.ServiceProvider;
        ApplicationDbContext = ServiceProvider.GetRequiredService<ApplicationDbContext>();
        HttpClient = integrationTestsWebApplicationFactory.CreateClient();
        JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        foreach (string? tableName in ApplicationDbContext.Model.GetEntityTypes().Select(entityType => entityType.GetTableName()))
        {
            if (!string.IsNullOrWhiteSpace(tableName))
            {
                string command = $"TRUNCATE TABLE \"{tableName}\" RESTART IDENTITY CASCADE;";
                await ApplicationDbContext.Database.ExecuteSqlRawAsync(command);
            }
        }
        _serviceScope.Dispose();
    }

    protected static async Task AssertProblemDetailsAsync(
        HttpResponseMessage httpResponseMessage,
        List<string> expectedErrors,
        ProblemHttpResult expectedProblemHttpResult)
    {
        ProblemDetails? actualValidationProblemDetails = await httpResponseMessage.Content.ReadFromJsonAsync<ProblemDetails>();
        List<string>? actualErrors = ((JsonElement)actualValidationProblemDetails!.Extensions["errors"]!).Deserialize<List<string>>();
        actualErrors.Should().BeEquivalentTo(expectedErrors);
        actualValidationProblemDetails!.Extensions["traceId"].Should().NotBeNull();
        actualValidationProblemDetails
            .Should()
            .BeEquivalentTo(
                expectedProblemHttpResult.ProblemDetails,
                options => options.Excluding(problemDetails => problemDetails.Extensions));
    }

    protected async Task AssertModelWithoutDateAuditsAsync<TModel>(HttpResponseMessage httpResponseMessage, TModel model)
        where TModel : class
    {
        httpResponseMessage.EnsureSuccessStatusCode();
        string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        TModel? actualRetrievedPostByIdDtos = JsonSerializer.Deserialize<TModel>(responseContent, JsonSerializerOptions);
        actualRetrievedPostByIdDtos
            .Should()
            .BeEquivalentTo(model, options => options
                .Using<DateTime>(assertionContext => assertionContext.Subject
                    .Should()
                    .BeCloseTo(assertionContext.Expectation, TimeSpan.FromMilliseconds(1)))
                .When(dto => dto.Path.EndsWith("CreatedAtUtc") || dto.Path.EndsWith("UpdatedAtUtc")));
    }
}
