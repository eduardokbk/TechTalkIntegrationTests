using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalkIntegrationTests.Application.Services;
using TechTalkIntegrationTests.Domain.Models.Dtos.Tasks;
using TechTalkIntegrationTests.Domain.Models.Entities;
using TechTalkIntegrationTests.Domain.Models.Enums;
using TechTalkIntegrationTests.Domain.Models.Repositories;
using TechTalkIntegrationTests.Domain.Models.Services;
using Xunit;

namespace TechTalkIntegrationTests.UnitTests.Application.Services
{
    public class TaskAppServiceTest
    {
        private readonly ITaskRepository _repository;
        private readonly ITwitterClientService _twitterClientService;
        private readonly TaskAppService _appService;

        public TaskAppServiceTest()
        {
            _repository = Substitute.For<ITaskRepository>();
            _twitterClientService = Substitute.For<ITwitterClientService>();
            _appService = new TaskAppService(_repository, _twitterClientService);
        }

        [Fact]
        public async Task GetAllAsync_ShouldCallRepository()
        {
            // Arrange 
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            _repository.GetAllAsNoTrackingAsync()
                .Returns(new List<TaskDomain>
                {
                    new TaskDomain(id1, "test 1", Priority.High, true),
                    new TaskDomain(id2, "test 2", Priority.Medium, false)
                });

            // Act
            var result = await _appService.GetAllAsync();

            // Assert
            await _repository.Received(1).GetAllAsNoTrackingAsync();
            result.Should().HaveCount(2);
            result[0].Id.Should().Be(id1);
            result[1].Id.Should().Be(id2);
        }

        [Fact]
        public async Task CreateAsync_WithValidValues_ShouldCreateTaskAndReturnId()
        {
            // Arrange
            var taskDto = new TaskForCreationDto
            {
                Completed = false,
                Description = "test",
                Priority = 1
            };

            var id = Guid.NewGuid();

            _repository.CreateAsync(default).ReturnsForAnyArgs(id);

            // Act
            var result = await _appService.CreateAsync(taskDto);

            // Assert
            await _repository.Received(1).CreateAsync(Arg.Is<TaskDomain>(x => x.Description == taskDto.Description &&
                                                                        x.Priority == (Priority)taskDto.Priority &&
                                                                        x.Completed == taskDto.Completed));
            await _repository.Received(1).SaveChangesAsync();
            result.Should().Be(id);
        }

        [Fact]
        public async Task CreateAsync_WithInvalidValues_ShouldThrowException()
        {
            // Arrange
            var taskDto = new TaskForCreationDto
            {
                Completed = false,
                Description = "",
                Priority = 1
            };

            var id = Guid.NewGuid();

            _repository.CreateAsync(default).ReturnsForAnyArgs(id);

            // Act
            Func<Task> act = async () => await _appService.CreateAsync(taskDto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*Description: cannot be empty!*");
            await _repository.DidNotReceiveWithAnyArgs().CreateAsync(default);
            await _repository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateAsync_WithValidValues_ShouldUpdateTask()
        {
            // Arrange
            var taskDomain = new TaskDomain(Guid.NewGuid(), "old", Priority.High, true);

            var newDescription = "";

            var taskDto = new TaskForCreationDto
            {
                Completed = taskDomain.Completed,
                Description = newDescription,
                Priority = (int)taskDomain.Priority
            };

            _repository.GetByIdAsNoTrackingAsync(Arg.Is(taskDomain.Id)).ReturnsForAnyArgs(taskDomain);

            // Act
            Func<Task> act = async () => await _appService.UpdateAsync(taskDomain.Id, taskDto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*Description: cannot be empty!*");
            _repository.DidNotReceiveWithAnyArgs().Update(default);
            await _repository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task CompleteAsync_WithValidId_ShouldCompleteTaskAndTweet()
        {
            // Arrange
            var taskDomain = new TaskDomain(Guid.NewGuid(), "old", Priority.High, false);

            _repository.GetByIdAsNoTrackingAsync(Arg.Is(taskDomain.Id)).ReturnsForAnyArgs(taskDomain);

            // Act
            await _appService.CompleteAsync(taskDomain.Id);

            // Assert
            _repository.Received(1).Update(Arg.Is<TaskDomain>(x => x.Description == taskDomain.Description &&
                                                                   x.Priority == taskDomain.Priority &&
                                                                   x.Completed &&
                                                                   x.Active == taskDomain.Active &&
                                                                   x.Id == taskDomain.Id));
            await _twitterClientService.Received(1).PostTweetAsync(Arg.Is("Task completed! \r \n #TechTalkIntegrationTest"));
            await _repository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task CompleteAsync_WithInValidId_ShouldThrowException()
        {
            // Arrange
            var taskDomain = new TaskDomain(Guid.NewGuid(), "old", Priority.High, false);

            _repository.GetByIdAsNoTrackingAsync(Arg.Is(taskDomain.Id)).ReturnsForAnyArgs(null as TaskDomain);

            // Act
            Func<Task> act = async () => await _appService.CompleteAsync(taskDomain.Id);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("Task not found!");
            _repository.DidNotReceiveWithAnyArgs().Update(default);
            await _repository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShoulDeleteTask()
        {
            // Arrange
            var taskDomain = new TaskDomain(Guid.NewGuid(), "old", Priority.High, false);

            _repository.GetByIdAsNoTrackingAsync(Arg.Is(taskDomain.Id)).ReturnsForAnyArgs(taskDomain);

            // Act
            await _appService.DeleteAsync(taskDomain.Id);

            // Assert
            _repository.Received(1).Delete(Arg.Is<TaskDomain>(x => x.Id == taskDomain.Id));
            await _repository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            var taskDomain = new TaskDomain(Guid.NewGuid(), "old", Priority.High, false);

            _repository.GetByIdAsNoTrackingAsync(Arg.Is(taskDomain.Id)).ReturnsForAnyArgs(null as TaskDomain);

            // Act
            Func<Task> act = async () => await _appService.CompleteAsync(taskDomain.Id);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("Task not found!");
            _repository.DidNotReceiveWithAnyArgs().Delete(default);
            await _repository.DidNotReceive().SaveChangesAsync();
        }
    }
}
