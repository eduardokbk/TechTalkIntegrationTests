using Xunit;

namespace TechTalkIntegrationTests.IntegrationTests.Configurations
{
    [CollectionDefinition("base integration test collection")]
    public class BaseIntegrationTestCollenction : ICollectionFixture<BaseTestFixture>
    {
    }
}
