using System.Collections.Generic;
using System.Threading.Tasks;
using Showtime.Mob.Models;

namespace Showtime.Mob.Interfaces
{
    public interface ITmdbApi
    {
        public Task<List<MovieDetails.Result>> GetTrendingMovies();
    }
}
