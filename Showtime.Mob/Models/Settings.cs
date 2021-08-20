using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Showtime.Mob.Models
{
    public class Settings
    {
        [JsonProperty("AppSettings")]
        public AppSettings appSettings { get; set; }

        [JsonProperty("ConnectionStrings")]
        public ConnectionStrings connectionStrings { get; set; }

        public class ProjectBaseUrls
        {
            [JsonProperty("AuthBaseUrl")]
            public string AuthBaseUrl { get; set; }
        }

        public class Tmdb
        {
            [JsonProperty("ApiToken")]
            public string ApiToken { get; set; }

            [JsonProperty("BaseUrl")]
            public string BaseUrl { get; set; }

            [JsonProperty("ImageBasePath")]
            public string ImageBasePath { get; set; }
        }

        public class ExternalApis
        {
            [JsonProperty("Tmdb")]
            public Tmdb Tmdb { get; set; }
        }

        public class JwtSettings
        {
            [JsonProperty("Audience")]
            public string Audience { get; set; }

            [JsonProperty("Issuer")]
            public string Issuer { get; set; }

            [JsonProperty("SecretKey")]
            public string SecretKey { get; set; }
        }

        public class AppSettings
        {
            [JsonProperty("ProjectBaseUrls")]
            public ProjectBaseUrls ProjectBaseUrls { get; set; }

            [JsonProperty("ExternalApis")]
            public ExternalApis ExternalApis { get; set; }

            [JsonProperty("JwtSettings")]
            public JwtSettings JwtSettings { get; set; }
        }

        public class ConnectionStrings
        {
            [JsonProperty("ShowtimeConnectionStringPublic")]
            public string ShowtimeConnectionStringPublic { get; set; }

            [JsonProperty("ShowtimeConnectionString")]
            public string ShowtimeConnectionString { get; set; }

            [JsonProperty("ShowtimeConnectionStringLocal")]
            public string ShowtimeConnectionStringLocal { get; set; }
        }


    }
}
