using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalkIntegrationTests.Domain.Models.Entities;
using TechTalkIntegrationTests.Domain.Models.Repositories;
using TechTalkIntegrationTests.Infrastructure.Context;

namespace TechTalkIntegrationTests.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        public readonly MainContext _dbContext;

        public BaseRepository(MainContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            return entity.Id;
        }

        public async Task<List<TEntity>> GetAllAsNoTrackingAsync()
        {
            return await _dbContext.Set<TEntity>()
                .Where(x => x.Active)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TEntity> GetByIdAsNoTrackingAsync(Guid id)
        {
            return await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }

        public async Task CreateManyAsync(IEnumerable<TEntity> entities)
        {
            await _dbContext.Set<TEntity>()
              .AddRangeAsync(entities);
        }

        public void Delete(TEntity entity)
        {
            entity.Inactivate();
            Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
