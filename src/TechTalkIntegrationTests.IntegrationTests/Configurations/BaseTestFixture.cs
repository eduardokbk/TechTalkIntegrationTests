using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;
using TechTalkIntegrationTests.Infrastructure.Context;
using TechTalkIntegrationTests.Web;

namespace TechTalkIntegrationTests.IntegrationTests.Configurations
{
    public class BaseTestFixture : IDisposable
    {
        public readonly HttpClient HttpClient;
        public readonly IServiceScope ServiceScope;
        public readonly MainContext Context;

        public BaseTestFixture()
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

            HttpClient = appFactory.CreateClient();
            ServiceScope = appFactory.Server.Services.CreateScope();
            Context = ServiceScope.ServiceProvider.GetService<MainContext>();
        }

        public void Dispose()
        {
            HttpClient.Dispose();
            Context.Dispose();
            ServiceScope.Dispose();
        }
    }
}