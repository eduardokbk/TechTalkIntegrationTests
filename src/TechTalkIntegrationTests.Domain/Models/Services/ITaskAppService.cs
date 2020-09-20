using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalkIntegrationTests.Domain.Models.Dtos.Tasks;

namespace TechTalkIntegrationTests.Domain.Models.Services
{
    public interface ITaskAppService
    {
        Task<List<TaskForResponseDto>> GetAllAsync();
        Task<Guid> CreateAsync(TaskForCreationDto taskForCreation);
        Task UpdateAsync(Guid id, TaskForCreationDto taskForCreation);
        Task CompleteAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
