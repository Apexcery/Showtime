using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Showtime.Web.Models.TmdbModels
{
    public class TrendingMoviesResult
    {
        public int Page { get; set; }
        
        [JsonPropertyName("results")]
        public List<BasicMovieDetails> Movies { get; set; }
        
        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
        
        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
    }
}
