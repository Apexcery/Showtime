﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showtime.Lib.Models.Auth
{
    public class RefreshResponse
    {
        public string UserId { get; set; }
        public string NewAccessToken { get; set; }
        public string NewRefreshToken { get; set; }
    }
}
