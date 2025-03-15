namespace GuildHub.IntegrationTests;

[CollectionDefinition(nameof(SharedDatabaseFixture))]
public sealed class SharedDatabaseFixture : ICollectionFixture<IntegrationTestsWebApplicationFactory>;
