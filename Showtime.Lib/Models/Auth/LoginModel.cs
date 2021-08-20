using System.ComponentModel.DataAnnotations;

namespace Showtime.Lib.Models.Auth
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username or Email is required.")]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
