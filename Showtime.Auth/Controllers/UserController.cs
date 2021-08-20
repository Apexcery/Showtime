using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Showtime.Auth.Services;
using Showtime.Lib.Models.User;

namespace Showtime.Auth.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(IUserService userService, UserManager<IdentityUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet("claims")]
        [Authorize]
        public IActionResult GetClaims()
        {
            var currentUser = _userService.GetCurrentClaimsIdentity(HttpContext.User.Identity);

            if (currentUser == null)
                return BadRequest();

            var claims = currentUser.Claims.Select(x => new Claim
            {
                Type = x.Type,
                Value = x.Value
            }).ToList();

            return Ok(claims);
        }

        [HttpGet("roles")]
        [Authorize]
        public async Task<IActionResult> GetRoles()
        {
            var currentUser = await _userService.GetCurrentIdentityUser(HttpContext.User.Identity);

            if (currentUser == null)
                return BadRequest();

            var roles = await _userService.GetCurrentUserRoles(currentUser);

            var rolesList = roles.Select(x => new Role
            {
                Name = x
            }).ToList();

            return Ok(rolesList);
        }

        [HttpGet("tokens")]
        [Authorize]
        public async Task<IActionResult> GetTokens()
        {
            var currentUser = await _userService.GetCurrentIdentityUser(HttpContext.User.Identity);

            if (currentUser == null)
                return BadRequest();

            Request.Headers.TryGetValue("Authorization", out var authHeader);

            var accessToken = authHeader.ToString().Replace("Bearer", string.Empty).Trim();
            var refreshToken = await _userManager.GetAuthenticationTokenAsync(currentUser, "Showtime.Auth", "RefreshToken");
            
            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
    }
}
