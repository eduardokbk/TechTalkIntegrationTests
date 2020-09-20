using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalkIntegrationTests.Domain.Models.Dtos.Tasks;
using TechTalkIntegrationTests.Domain.Models.Entities;
using TechTalkIntegrationTests.Domain.Models.Enums;
using TechTalkIntegrationTests.Domain.Models.Repositories;
using TechTalkIntegrationTests.Domain.Models.Services;

namespace TechTalkIntegrationTests.Application.Services
{
    public class TaskAppService : ITaskAppService
    {
        private readonly ITaskRepository _repository;

        public TaskAppService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TaskForResponseDto>> GetAllAsync()
        {
            var tasks = await _repository.GetAllAsNoTrackingAsync();

            return tasks.Select(x => new TaskForResponseDto
            {
                Id = x.Id,
                Description = x.Description,
                Priority = (int)x.Priority,
                Completed = x.Completed
            }).ToList();
        }

        public async Task<Guid> CreateAsync(TaskForCreationDto taskForCreation)
        {
            var taskDomain = new TaskDomain(Guid.NewGuid(), taskForCreation.Description, (Priority)taskForCreation.Priority, taskForCreation.Completed);

            taskDomain.Validate();

            var id = await _repository.CreateAsync(taskDomain);

            await _repository.SaveChangesAsync();

            return id;
        }

        public async Task UpdateAsync(Guid id, TaskForCreationDto taskForCreation)
        {
            var task = await GetAndValidateAsync(id);

            task.Update(taskForCreation.Description, taskForCreation.Completed, (Priority)taskForCreation.Priority);

            task.Validate();

            _repository.Update(task);

            await _repository.SaveChangesAsync();
        }

        public async Task CompleteAsync(Guid id)
        {
            TaskDomain task = await GetAndValidateAsync(id);

            task.Complete();
            _repository.Update(task);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            _repository.Delete(await GetAndValidateAsync(id));
            await _repository.SaveChangesAsync();
        }

        private async Task<TaskDomain> GetAndValidateAsync(Guid id)
        {
            var task = await _repository.GetByIdAsNoTrackingAsync(id);

            if (task == null)
                throw new ArgumentException("Task not found!");

            return task;
        }
    }
}
