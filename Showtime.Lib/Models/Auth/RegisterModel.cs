using System.ComponentModel.DataAnnotations;

namespace Showtime.Lib.Models.Auth
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Forename is required.")]
        public string Forename { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
    }
}
