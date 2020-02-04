using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Hangfire.Storage;
using HangFire_Core.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace HangFire_Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
            });

            //Hangfire
            var optionsHangFire = new SqlServerStorageOptions
            {
                JobExpirationCheckInterval = TimeSpan.FromDays(1),
                //PrepareSchemaIfNecessary = false
            };
            services.AddHangfire(config =>
                config.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), optionsHangFire));


            //Authorization
            services.Configure<AuthorizationOptions>(options =>
            {
                options.AddPolicy("somePolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();

                    // Maybe require a claim here, if you need that.
                    //policy.RequireClaim(ClaimTypes.Role, "some role claim");
                });
            });
            GlobalJobFilters.Filters.Add(new HangFireExpirationTimeAttribute());

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            //Hangfire
            //app.UseHangfireDashboard();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new IDashboardAuthorizationFilter[] {
                    new HangfireAuthorizationFilter("somePolicy")
                }
            });

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                ServerName = "Vlotte",
                Queues = new[] {"default" },
                //Default job recurring with 15s, using SchedulePollingInterval to set < 15s
                SchedulePollingInterval = TimeSpan.FromMilliseconds(1000)
            });

            //var monitoringApi = JobStorage.Current.GetMonitoringApi();
            //var serverToRemove = monitoringApi.Servers().First(svr => svr.Name.Contains("Vlotte"));
            //JobStorage.Current.GetConnection().RemoveServer(serverToRemove.Name);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
