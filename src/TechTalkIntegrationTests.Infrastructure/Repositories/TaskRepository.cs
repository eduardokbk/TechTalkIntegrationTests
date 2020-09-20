using TechTalkIntegrationTests.Domain.Models.Entities;
using TechTalkIntegrationTests.Domain.Models.Repositories;
using TechTalkIntegrationTests.Infrastructure.Context;

namespace TechTalkIntegrationTests.Infrastructure.Repositories
{
    public class TaskRepository : BaseRepository<TaskDomain>, ITaskRepository
    {
        public TaskRepository(MainContext dbContext) : base(dbContext)
        {
        }
    }
}
