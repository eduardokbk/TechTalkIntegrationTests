using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechTalkIntegrationTests.Domain.Models.Dtos.Tasks;
using TechTalkIntegrationTests.Domain.Models.Entities;
using TechTalkIntegrationTests.Domain.Models.Enums;
using TechTalkIntegrationTests.IntegrationTests.Configurations;
using Xunit;

namespace TechTalkIntegrationTests.IntegrationTests.Controllers
{
    public class TaskControllersTest : BaseIntegrationTest
    {
        public TaskControllersTest(BaseTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Get_WithoutDataInDatabase_ShouldReturnEmptyList()
        {
            // Arrange
            var url = "api/task";

            // Act
            var response = await _httpClient.GetAsync(url);

            //Assert
            response.EnsureSuccessStatusCode();

            var content = JsonConvert.DeserializeObject<List<TaskForResponseDto>>(await response.Content.ReadAsStringAsync());

            content.Should().BeEmpty();
        }

        [Fact]
        public async Task WithDataInDatabase_ShouldReturnIt()
        {
            // Arrange
            var url = "api/task";

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            await CreateDataAsync(new TaskDomain(id1, "get test", Priority.High, true));
            await CreateDataAsync(new TaskDomain(id2, "get test", Priority.Low, true));
            await CreateDataAsync(new TaskDomain(id3, "get test", Priority.Medium, false));


            // Act
            var response = await _httpClient.GetAsync(url);

            //Assert
            response.EnsureSuccessStatusCode();

            var content = JsonConvert.DeserializeObject<List<TaskForResponseDto>>(await response.Content.ReadAsStringAsync());

            content.Should().HaveCount(3);
            content.Select(x => x.Id).Should().Contain(id1);
            content.Select(x => x.Id).Should().Contain(id2);
            content.Select(x => x.Id).Should().Contain(id3);
        }

        [Fact]
        public async Task Create_WithValidDto_ShouldCreateWithSuccess()
        {
            // Arrange
            var url = "api/task";
            var dto = new TaskForCreationDto
            {
                Completed = false,
                Description = "integration test",
                Priority = (int)Priority.Low
            };

            var serialiedDto = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PostAsync(url, serialiedDto);

            //Assert
            response.EnsureSuccessStatusCode();

            var id = JsonConvert.DeserializeObject<Guid>(await response.Content.ReadAsStringAsync());

            id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task Create_WithInvalidDto_ShouldReturnError()
        {
            // Arrange
            var url = "api/task";
            var dto = new TaskForCreationDto();

            var serialiedDto = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PostAsync(url, serialiedDto);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Update_WithValidDto_ShouldUpdateWithSuccess()
        {
            // Arrange
            var taskDto = new TaskForCreationDto
            {
                Completed = false,
                Description = "integration test",
                Priority = (int)Priority.Low
            };

            var id = Guid.NewGuid();
            await CreateDataAsync(new TaskDomain(id, "test update", Priority.High, true));

            var url = $"api/task/{id}";

            taskDto.Description = "update";

            var serialiedDto = new StringContent(JsonConvert.SerializeObject(taskDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PutAsync(url, serialiedDto);

            //Assert
            response.EnsureSuccessStatusCode();
            (await GetDataAsync<TaskDomain>(id)).Description.Should().Be(taskDto.Description);
        }

        [Fact]
        public async Task Update_WithInvalidDto_ShouldReturnError()
        {
            // Arrange
            var taskDto = new TaskForCreationDto
            {
                Completed = false,
                Description = "integration test",
                Priority = (int)Priority.Low
            };

            var id = Guid.NewGuid();
            await CreateDataAsync(new TaskDomain(id, "test update", Priority.High, true));

            var url = $"api/task/{id}";

            taskDto.Description = "";

            var serialiedDto = new StringContent(JsonConvert.SerializeObject(taskDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PutAsync(url, serialiedDto);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Complete_WithValidId_ShouldCompleteTask()
        {
            // Arrange
            var id = Guid.NewGuid();
            await CreateDataAsync(new TaskDomain(id, "test", Priority.High, false));

            var url = $"api/task/{id}/complete";

            // Act
            var response = await _httpClient.PatchAsync(url, null);

            //Assert
            response.EnsureSuccessStatusCode();

            (await GetDataAsync<TaskDomain>(id)).Completed.Should().BeTrue();
        }
    }
}
