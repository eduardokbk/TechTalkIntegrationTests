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

            app.UseCors(option => option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<MainContext>();
                context.Database.Migrate();
            }

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
