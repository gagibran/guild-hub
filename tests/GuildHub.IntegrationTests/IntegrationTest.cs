namespace GuildHub.IntegrationTests;

public abstract class IntegrationTest(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
    : IClassFixture<IntegrationTestsWebApplicationFactory>
{
    protected HttpClient HttpClient { get; } = integrationTestsWebApplicationFactory.CreateClient();

    protected JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    protected async Task<TReturn> GetAsync<TReturn>(string uri) where TReturn : class
    {
        HttpClient.DefaultRequestHeaders.Add("Accept", MediaTypeNames.Application.Json);
        HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync(uri);
        HttpClient.DefaultRequestHeaders.Clear();
        string content = await httpResponseMessage.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TReturn>(content, JsonSerializerOptions)!;
    }
}
