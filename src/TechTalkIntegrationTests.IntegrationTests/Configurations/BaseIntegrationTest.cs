using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalkIntegrationTests.Domain.Models.Entities;
using TechTalkIntegrationTests.Infrastructure.Context;
using Xunit;

namespace TechTalkIntegrationTests.IntegrationTests.Configurations
{
    [Collection("base integration test collection")]
    public abstract class BaseIntegrationTest
    {
        private readonly IServiceScope _serviceScope;
        protected readonly HttpClient _httpClient;
        protected readonly MainContext _context;
        protected readonly BaseTestFixture _fixture;

        protected BaseIntegrationTest(BaseTestFixture fixture)
        {
            _fixture = fixture;
            _httpClient = _fixture.HttpClient;
            _serviceScope = _fixture.ServiceScope;
            _context = _fixture.Context;
                  
            ClearDb().Wait();
        }

        private async Task ClearDb()
        {
            _context.RemoveRange(_context.Set<TaskDomain>());
            await _context.SaveChangesAsync();
        }

        protected TService GetService<TService>()
        {
            return _serviceScope.ServiceProvider.GetService<TService>();
        }

        protected async Task CreateDataAsync<TEntity>(TEntity entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        protected async Task<TEntity> GetDataAsync<TEntity>(Guid id) 
            where TEntity : BaseEntity
        {
            return await _context.Set<TEntity>().AsNoTracking().FirstAsync(x => x.Id == id);
        }
    }
}
