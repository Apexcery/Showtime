using System.Collections.Generic;
using Showtime.Web.Models.TmdbModels;

namespace Showtime.Web.ViewModels
{
    public class HomeViewModel
    {
        public IList<BasicMediaDetails> Movies { get; set; } = new List<BasicMediaDetails>();
        public IList<BasicMediaDetails> Tv { get; set; } = new List<BasicMediaDetails>();
    }
}
