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

    protected async Task<TReturn> CreateAsync<TReturn>(string content, string uri)
        where TReturn : class
    {
        var stringContent = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);
        var httpResponseMessage = await HttpClient.PostAsync(uri, stringContent);
        string entity = await httpResponseMessage.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TReturn>(entity, JsonSerializerOptions)!;
    }
}
