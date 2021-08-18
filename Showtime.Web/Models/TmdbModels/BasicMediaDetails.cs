using System.Collections.Generic;
using Newtonsoft.Json;

namespace Showtime.Web.Models.TmdbModels
{
    public class BasicMediaDetails
    {
        [JsonIgnore]
        public string BackdropBasePath { get; set; }
        [JsonIgnore]
        public string PosterBasePath { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }
        [JsonProperty("name")]
        private string Name {
            get => Title;
            set => Title = value;
        }


        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }
        [JsonProperty("original_name")]
        private string OriginalName 
        {
            get => OriginalTitle;
            set => OriginalTitle = value;
        }
        

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }
        [JsonProperty("first_air_date")]
        private string FirstAirDate
        {
            get => ReleaseDate;
            set => ReleaseDate = value;
        }



        public string Overview { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("genre_ids")]
        public List<int> GenreIds { get; set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        public double Popularity { get; set; }


        // movie only
        public bool Adult { get; set; }

        public bool Video { get; set; }


        // tv only
        [JsonProperty("origin_country")]
        public List<string> OriginCountry { get; set; }
    }
}
