using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Showtime.Web.Data;
using Showtime.Web.Enums.Tmdb;
using Showtime.Web.Models.TmdbModels;

namespace Showtime.Web.Services
{
    public interface ITmdbService
    {
        public Task<IList<BasicMovieDetails>> GetTrendingMovies(TimeWindow timeWindow);
    }

    public class TmdbService : ITmdbService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public TmdbService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }

        public async Task<IList<BasicMovieDetails>> GetTrendingMovies(TimeWindow timeWindow)
        {
            var result = await _client.GetAsync(@$"trending/movie/{timeWindow}");

            if (!result.IsSuccessStatusCode)
                return null;

            var movieList = (await result.Content.ReadFromJsonAsync<TrendingMoviesResult>()).Movies
                .Select(movie =>
                {
                    var tmdbApi = _config.GetSection("AppSettings:ExternalApis").Get<ExternalApis>().Tmdb;
                    
                    movie.BackdropBasePath = $"{tmdbApi.ImageBasePath}{BackdropSize.w780}/";
                    movie.PosterBasePath = $"{tmdbApi.ImageBasePath}{PosterSize.w500}/";
                    
                    return movie;
                }).ToList();

            return movieList;
        }
    }
}
