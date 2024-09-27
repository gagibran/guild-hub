namespace GuildHub.IntegrationTests;

public abstract class IntegrationTest(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
    : IClassFixture<IntegrationTestsWebApplicationFactory>
{
    protected HttpClient HttpClient { get; } = integrationTestsWebApplicationFactory.CreateClient();
}
