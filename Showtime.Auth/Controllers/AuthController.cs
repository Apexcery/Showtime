using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Showtime.Lib.Models.Auth;

namespace Showtime.Auth.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _userManager.FindByNameAsync(model.Username) != null)
                return BadRequest("Username is already in use.");

            var user = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(x => $"{x.Code} : {x.Description}").ToList());
            
            if (_userManager.Users.Count() == 1)
                await _userManager.AddToRoleAsync(user, "Admin");
                
            await _userManager.AddToRoleAsync(user, "User");

            return Ok(new
            {
                message = "Successfully Registered",
                user = new RegisterResponse
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email
                }
            });

        }
    }
}
