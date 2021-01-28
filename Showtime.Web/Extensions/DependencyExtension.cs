using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Showtime.Web.Data;
using Showtime.Web.Services;

namespace Showtime.Web.Extensions
{
    public static class DependencyExtension
    {
        public static IServiceCollection InjectDependencies(this IServiceCollection services, IConfiguration config)
        {
            var projectBaseUrls = config.GetSection("AppSettings:ProjectBaseUrls").Get<ProjectBaseUrls>();

            services.AddHttpClient<IAuthService, AuthService>("AuthApi", client =>
            {
                client.BaseAddress = new Uri(projectBaseUrls.AuthBaseUrl);
            });

            return services;
        }
    }
}
