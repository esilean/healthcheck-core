using HealthCheck.MicroA.Api.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;

namespace HealthCheck.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();


            services.AddSingleton<LogFileHealthCheck>();
            services.AddSingleton<TripwireHealthCheck>();
            services.AddSingleton<SlowDependencyHealthCheck>();
            services.AddSingleton(new ForcedHealthCheck(Configuration["HealthInitialState"]));

            services.AddHealthChecks()
                    .AddSqlServer(
                        connectionString: Configuration.GetConnectionString("DefaultConnection"),
                        name: "Sql Server DB",
                        timeout: TimeSpan.FromSeconds(5),
                        failureStatus: HealthStatus.Unhealthy)
                    .AddMongoDb(
                        mongodbConnectionString: Configuration.GetSection("MongoDb:ConnectionString").Value,
                        name: "Mongo",
                        timeout: TimeSpan.FromSeconds(5),
                        failureStatus: HealthStatus.Unhealthy)
                    .AddCheck<LogFileHealthCheck>("LogFile")
                    .AddCheck<ForcedHealthCheck>("Forceable")
                    .AddCheck<SlowDependencyHealthCheck>("SlowDependency", tags: new string[] { "ready" })
                    .AddCheck<TripwireHealthCheck>("Tripwire", failureStatus: HealthStatus.Degraded);

            services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(5); //time in seconds between check
                opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
                opt.SetApiMaxActiveRequests(1); //api requests concurrency

                opt.AddHealthCheckEndpoint("Micro Api A", "/health/lively"); //map health check api
            })
            .AddInMemoryStorage();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //app.UseWhen(
            //    ctx => ctx.User.Identity.IsAuthenticated,
            //    a => a.UseHealthChecks("/securehealth", new HealthCheckOptions() { Predicate = _ => false })
            //);


            app.UseRouting();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/lively", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = reg => reg.Tags.Contains("ready"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                }).RequireHost("localhost:5001");

                //endpoints.MapHealthChecksUI(); //http://{YOUR-SERVER}/healthchecks-ui
                endpoints.MapHealthChecksUI(setup =>
                {
                    setup.UIPath = "/healthchecks-ui"; // UI path in browser
                    setup.ApiPath = "/healthchecks-ui-api"; // API of SPA application
                });

                endpoints.MapControllers();
            });
        }
    }
}
