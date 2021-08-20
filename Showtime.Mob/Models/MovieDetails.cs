using System.Collections.Generic;
using Newtonsoft.Json;

namespace Showtime.Mob.Models
{
    public class MovieDetails
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }

        public class Result
        {
            [JsonProperty("release_date")]
            public string ReleaseDate { get; set; }

            [JsonProperty("adult")]
            public bool Adult { get; set; }

            [JsonProperty("backdrop_path")]
            public string BackdropPath { get; set; }

            [JsonProperty("genre_ids")]
            public List<int> GenreIds { get; set; }

            [JsonProperty("vote_count")]
            public int VoteCount { get; set; }

            [JsonProperty("original_language")]
            public string OriginalLanguage { get; set; }

            [JsonProperty("original_title")]
            public string OriginalTitle { get; set; }

            [JsonProperty("poster_path")]
            public string PosterPath { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("video")]
            public bool Video { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("vote_average")]
            public double VoteAverage { get; set; }

            [JsonProperty("overview")]
            public string Overview { get; set; }

            [JsonProperty("popularity")]
            public double Popularity { get; set; }

            [JsonProperty("media_type")]
            public string MediaType { get; set; }
        }
    }
}
