using System.Collections.Generic;

namespace Showtime.Web.Models
{
    public class LoginValidationError
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
        public Errors Errors { get; set; }
    }

    public class Errors
    {
        public List<string> Password { get; set; }
        public List<string> UsernameOrEmail { get; set; }
    }
}
