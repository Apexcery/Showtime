using System;
using System.Net.Http.Headers;
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
            services.InjectProjectBaseClients(config);

            services.InjectTmdbClient(config);

            return services;
        }

        private static IServiceCollection InjectProjectBaseClients(this IServiceCollection services, IConfiguration config)
        {
            var projectBaseUrls = config.GetSection("AppSettings:ProjectBaseUrls").Get<ProjectBaseUrls>();

            services.AddHttpClient<IAuthService, AuthService>("AuthApi", client =>
            {
                client.BaseAddress = new Uri(projectBaseUrls.AuthBaseUrl);
            });

            return services;
        }

        private static IServiceCollection InjectTmdbClient(this IServiceCollection services, IConfiguration config)
        {
            var tmdbApi = config.GetSection("AppSettings:ExternalApis").Get<ExternalApis>().Tmdb;

            services.AddHttpClient<ITmdbService, TmdbService>("TmdbApi", client =>
            {
                client.BaseAddress = new Uri(tmdbApi.BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tmdbApi.ApiToken);
            });

            return services;
        }
    }
}
