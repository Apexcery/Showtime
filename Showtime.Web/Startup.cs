using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Microsoft.Extensions.Hosting;
using Showtime.Web.Data;
using Showtime.Web.Services;

namespace Showtime.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            // Configuration = configuration;
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(Path.GetFullPath(@"../Showtime.Settings/appSettings.json"), false, true)
                .AddJsonFile(Path.GetFullPath(@$"../Showtime.Settings/appSettings.{env.EnvironmentName}.json"), true, true)
                .Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(provider => Configuration);

            var projectBaseUrls = Configuration.GetSection("AppSettings:ProjectBaseUrls").Get<ProjectBaseUrls>();
            
            services.AddHttpClient<IAuthService, AuthService>("AuthApi", client =>
            {
                client.BaseAddress = new Uri(projectBaseUrls.AuthBaseUrl);
            });

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
