using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalkIntegrationTests.Domain.Models.Entities;

namespace TechTalkIntegrationTests.Domain.Models.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<Guid> CreateAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsNoTrackingAsync();
        Task<TEntity> GetByIdAsNoTrackingAsync(Guid id);
        void Update(TEntity entity);
        Task CreateManyAsync(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        Task SaveChangesAsync();
    }
}
