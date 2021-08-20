using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Showtime.Mob.Interfaces;
using Showtime.Mob.Models;

namespace Showtime.Mob.Services
{
    public class TmdbAPI : ITmdbApi
    {
        private readonly HttpClient _client;

        public TmdbAPI(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<MovieDetails.Result>> GetTrendingMovies()
        {
            var response = await _client.GetAsync($"trending/movie/day");
            if (!response.IsSuccessStatusCode)
                return null;

            var allMovies = JsonConvert.DeserializeObject<MovieDetails>(await response.Content.ReadAsStringAsync());
            var trendingMovies = allMovies?.Results;

            return trendingMovies;
        }
    }
}
