﻿using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Showtime.Web.Data;
using Showtime.Web.Enums.Tmdb;

namespace Showtime.Web.Models.TmdbModels
{
    public class BasicMovieDetails
    {
        [JsonIgnore]
        public string BackdropBasePath { get; set; }
        [JsonIgnore]
        public string PosterBasePath { get; set; }

        public bool Adult { get; set; }

        [JsonPropertyName("backdrop_path")]
        public string BackdropPath { get; set; }
        
        [JsonPropertyName("genre_ids")]
        public List<int> GenreIds { get; set; }
        
        public int Id { get; set; }
        
        [JsonPropertyName("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonPropertyName("original_title")]
        public string OriginalTitle { get; set; }
        
        public string Overview { get; set; }

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; }

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; }
        
        public string Title { get; set; }
        
        public bool Video { get; set; }

        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; set; }

        [JsonPropertyName("vote_count")]
        public int VoteCount { get; set; }
        
        public double Popularity { get; set; }

        [JsonPropertyName("media_type")]
        public string MediaType { get; set; }
    }
}
