using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalkIntegrationTests.Domain.Models.Entities;
using TechTalkIntegrationTests.Infrastructure.Context;
using TechTalkIntegrationTests.Web;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Configuration;

namespace TechTalkzaoIntegrationTest.Base
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
                        services.AddDbContext<MainContext>(x => x.UseSqlServer(Startup.Configuration.GetConnectionString("tech-talk-connection-test")));
                    });
                });

            _httpClient = appFactory.CreateClient();
            _serviceScope = appFactory.Server.Services.CreateScope();
            _mainContext = GetService<MainContext>();

            ClearDb();
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

        protected void ClearDb()
        {
            _mainContext.RemoveRange(_mainContext.Set<TaskDomain>());
            _mainContext.SaveChanges();
        }
    }
}
