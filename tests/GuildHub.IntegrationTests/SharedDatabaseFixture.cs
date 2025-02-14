namespace GuildHub.IntegrationTests;

[CollectionDefinition(nameof(SharedDatabaseFixture))]
public class SharedDatabaseFixture : ICollectionFixture<IntegrationTestsWebApplicationFactory>;
