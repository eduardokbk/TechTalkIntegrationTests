using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TechTalkIntegrationTests.Application.Services;
using TechTalkIntegrationTests.Domain.Models.Repositories;
using TechTalkIntegrationTests.Domain.Models.Services;
using TechTalkIntegrationTests.Infrastructure.Context;
using TechTalkIntegrationTests.Infrastructure.Repositories;
using TechTalkIntegrationTests.Infrastructure.Services;
using TechTalkIntegrationTests.Web.Filters;
using Tweetinvi;
using Tweetinvi.Models;

namespace TechTalkIntegrationTests.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDependencies(services);
            services.AddControllers();
            services.AddMvc(opt =>
            {
                opt.EnableEndpointRouting = false;
                opt.Filters.Add<ExceptionFilter>();
            });
            services.AddDbContext<MainContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("tech-talk-connection")));
            Auth.SetCredentials(new TwitterCredentials("1OgklADZZ0dmsfL0BlEjSIcLH", "eHgEjBh55vF20xXvU7j6JuCCA5c2qhFtE1DSxXVadPbDfaJC9d", "764558540497362949-yxvNeGPhwsNE02UQETiY3mfHoD01OXt", "TvC1qQ6TSpBMuHYZRjy8mTKiQmlMJoSZcaIBuMfiE29PE"));
        }

        private void ConfigureDependencies(IServiceCollection services)
        {
            services.AddTransient<ITaskAppService, TaskAppService>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<ITwitterClientService, TwitterClientService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<MainContext>();
                context.Database.Migrate();
            }

            app.UseCors(option => option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
