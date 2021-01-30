using System.Collections.Generic;
using Newtonsoft.Json;

namespace Showtime.Web.Models.TmdbModels.ApiResults
{
    public class TrendingMoviesApiResult
    {
        public int Page { get; set; }
        
        [JsonProperty("results")]
        public List<BasicMediaDetails> Movies { get; set; }
        
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
        
        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
    }
}
