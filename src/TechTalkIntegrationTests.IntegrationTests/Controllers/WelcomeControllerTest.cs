using FluentAssertions;
using System.Threading.Tasks;
using TechTalkIntegrationTests.IntegrationTests.Configurations;
using Xunit;

namespace TechTalkIntegrationTests.IntegrationTests.Controllers
{
    public class WelcomeControllerTest : BaseIntegrationTest
    {
        [Fact]
        public async Task Get_ShouldReturnWelcomeWithSucces()
        {
            // Arrange
            var url = "api/Welcome";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("Welcome");
        }
    }
}
