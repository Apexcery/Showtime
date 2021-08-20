using System.Collections.Generic;
using Newtonsoft.Json;

namespace Showtime.Web.Models.TmdbModels.ApiResults
{
    public class SearchApiResult
    {
        public int Page { get; set; }

        public List<BasicMediaDetails> Results { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
    }
}
