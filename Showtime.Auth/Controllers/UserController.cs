using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Showtime.Lib.Models.User;
using Showtime.Lib.Services;

namespace Showtime.Auth.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("claims")]
        [Authorize]
        public IActionResult GetClaims()
        {
            var currentUser = _userService.GetCurrentClaimsIdentity(HttpContext.User.Identity);

            if (currentUser == null)
                return BadRequest();

            var claims = currentUser.Claims.Select(x => new Lib.Models.User.Claim
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
    }
}
