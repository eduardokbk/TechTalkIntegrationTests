using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalkIntegrationTests.Domain.Models.Entities;
using TechTalkIntegrationTests.Infrastructure.Context;
using TechTalkIntegrationTests.Web;

namespace TechTalkIntegrationTests.IntegrationTests.Configurations
{
    public class BaseIntegrationTest
    {
        protected readonly HttpClient _httpClient;
        protected readonly MainContext _mainContext;
        private readonly IServiceScope _serviceScope;

        public BaseIntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<MainContext>));

                        services.AddDbContext<MainContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryDbForTesting");
                        });
                    });
                });

            _httpClient = appFactory.CreateClient();
            _serviceScope = appFactory.Server.Services.CreateScope();
            _mainContext = GetService<MainContext>();
        }

        protected TService GetService<TService>()
        {
            return _serviceScope.ServiceProvider.GetService<TService>();
        }

        protected async Task CreateDataAsync<TEntity>(TEntity entity)
        {
            await _mainContext.AddAsync(entity);
            await _mainContext.SaveChangesAsync();
        }

        protected async Task<TEntity> GetDataAsync<TEntity>(Guid id)
            where TEntity : BaseEntity
        {
            return await _mainContext.Set<TEntity>().AsNoTracking().FirstAsync(x => x.Id == id);
        }
    }
}
