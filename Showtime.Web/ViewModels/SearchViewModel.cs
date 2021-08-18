using System.Collections.Generic;
using Showtime.Web.Models.TmdbModels;

namespace Showtime.Web.ViewModels
{
    public class SearchViewModel
    {
        public string SearchQuery { get; set; }

        public IList<BasicMediaDetails> SearchResults { get; set; } = new List<BasicMediaDetails>();
    }
}