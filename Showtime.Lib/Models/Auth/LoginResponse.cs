using System;

namespace Showtime.Lib.Models.Auth
{
    public class LoginResponse
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
    }
}
