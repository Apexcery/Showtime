using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Showtime.Web.Data;
using Showtime.Web.Enums.Tmdb;
using Showtime.Web.Models.TmdbModels;
using Showtime.Web.Models.TmdbModels.ApiResults;
using Showtime.Web.Models.TmdbModels.FullDetails;

namespace Showtime.Web.Services
{
    public interface ITmdbService
    {
        public Task<IList<BasicMediaDetails>> GetTrendingMovies(TimeWindow timeWindow);
        public Task<IList<BasicMediaDetails>> GetTrendingTv(TimeWindow timeWindow);
        public Task<IList<BasicMediaDetails>> Search(string searchQuery);
        public Task<Movie> GetFullMovieDetails(int movieId);
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

        public async Task<IList<BasicMediaDetails>> GetTrendingMovies(TimeWindow timeWindow)
        {
            var result = await _client.GetAsync(@$"trending/movie/{timeWindow}");

            if (!result.IsSuccessStatusCode)
                return null;

            var jsonString = await result.Content.ReadAsStringAsync();

            var tmdbApi = _config.GetSection("AppSettings:ExternalApis").Get<ExternalApis>().Tmdb;
            var movieList = JsonConvert.DeserializeObject<TrendingMoviesApiResult>(jsonString).Movies
                .Select(movie =>
                {
                    movie.BackdropBasePath = $"{tmdbApi.ImageBasePath}{BackdropSize.w780}";
                    movie.PosterBasePath = $"{tmdbApi.ImageBasePath}{PosterSize.w500}";
                
                    return movie;
                }).ToList();

            return movieList;
        }

        public async Task<IList<BasicMediaDetails>> GetTrendingTv(TimeWindow timeWindow)
        {
            var result = await _client.GetAsync(@$"trending/tv/{timeWindow}");

            if (!result.IsSuccessStatusCode)
                return null;

            var jsonString = await result.Content.ReadAsStringAsync();

            var tmdbApi = _config.GetSection("AppSettings:ExternalApis").Get<ExternalApis>().Tmdb;
            var tvList = JsonConvert.DeserializeObject<TrendingTvApiResult>(jsonString).Tv
                .Select(tv =>
                {
                    tv.BackdropBasePath = $"{tmdbApi.ImageBasePath}{BackdropSize.w780}";
                    tv.PosterBasePath = $"{tmdbApi.ImageBasePath}{PosterSize.w500}";

                    return tv;
                }).ToList();

            return tvList;
        }

        public async Task<IList<BasicMediaDetails>> Search(string searchQuery)
        {
            var includeAdult = false;

            var result = await _client.GetAsync(@$"search/multi?query=snowpiercer&page=1&include_adult={includeAdult}");

            if (!result.IsSuccessStatusCode)
                return null;

            var jsonString = await result.Content.ReadAsStringAsync();

            var tmdbApi = _config.GetSection("AppSettings:ExternalApis").Get<ExternalApis>().Tmdb;
            var searchResults = JsonConvert.DeserializeObject<SearchApiResult>(jsonString).Results
                .Select(result =>
                {
                    result.BackdropBasePath = $"{tmdbApi.ImageBasePath}{BackdropSize.w780}";
                    result.PosterBasePath = $"{tmdbApi.ImageBasePath}{PosterSize.w500}";

                    return result;
                }).ToList();

            return searchResults;
        }

        public async Task<Movie> GetFullMovieDetails(int movieId)
        {
            var result = await _client.GetAsync($@"movie/{movieId}");

            if (!result.IsSuccessStatusCode)
                return null;

            var jsonString = await result.Content.ReadAsStringAsync();

            var movieDetails = JsonConvert.DeserializeObject<Movie>(jsonString);

            var tmdbApi = _config.GetSection("AppSettings:ExternalApis").Get<ExternalApis>().Tmdb;

            movieDetails.BackdropBasePath = $"{tmdbApi.ImageBasePath}{BackdropSize.w780}";
            movieDetails.PosterBasePath = $"{tmdbApi.ImageBasePath}{PosterSize.w500}";

            return movieDetails;
        }
    }
}
