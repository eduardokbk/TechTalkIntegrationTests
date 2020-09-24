using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalkIntegrationTests.Domain.Models.Dtos.Tasks;
using TechTalkIntegrationTests.Domain.Models.Entities;
using TechTalkzaoIntegrationTest.Base;
using Xunit;

namespace TechTalkzaoIntegrationTest.Controller
{
    public class TaskControllerTest : BaseIntegrationTest
    {
        [Fact]
        public async Task Get_WhenHasNoDataInBase_ShouldReturnEmptyList()
        {
            // Arrange
            var url = "api/task";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            var listTaskForResponseDto = JsonConvert.DeserializeObject<List<TaskForResponseDto>>(
                await response.Content.ReadAsStringAsync()
            );
            response.EnsureSuccessStatusCode();
            listTaskForResponseDto.Should().BeEmpty();
        }

        [Fact]
        public async Task Get_WhenHasDataInBase_ShouldReturnList()
        {
            // Arrange
            var url = "api/task";
            var taskDomain = new TaskDomain(Guid.NewGuid(), "Luanzao gostosao", TechTalkIntegrationTests.Domain.Models.Enums.Priority.High, false);

            await CreateDataAsync(taskDomain);

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            var listTaskForResponseDto = JsonConvert.DeserializeObject<List<TaskForResponseDto>>(
                await response.Content.ReadAsStringAsync()
            );
            response.EnsureSuccessStatusCode();
            listTaskForResponseDto.Should().NotBeEmpty();
        }
    }
}
