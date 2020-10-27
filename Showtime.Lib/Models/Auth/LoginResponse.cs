using System;

namespace Showtime.Lib.Models.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Id { get; set; }
    }
}
