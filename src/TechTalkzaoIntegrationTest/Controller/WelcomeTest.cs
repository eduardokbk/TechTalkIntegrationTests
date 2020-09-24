using Xunit;
using TechTalkzaoIntegrationTest.Base;
using System.Threading.Tasks;
using FluentAssertions;

namespace TechTalkzaoIntegrationTest
{
    public class WelcomeTest : BaseIntegrationTest
    {
        [Fact]
        public async Task Get_ReturnExpected()
        {
            // Arrange
            var url = "api/welcome";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("Welcome");
        }
    }
}
