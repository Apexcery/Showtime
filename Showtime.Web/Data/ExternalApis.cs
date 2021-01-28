using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showtime.Web.Data
{
    public class ExternalApis
    {
        public TmdbApi Tmdb { get; set; }

        public class TmdbApi
        {
            public string BaseUrl { get; set; }
            public string ApiToken { get; set; }
            public string ImageBasePath { get; set; }
        }
    }
}
