using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Showtime.Lib.Models;
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
                return BadRequest(new Error{
                    StatusCode = StatusCodes.Status409Conflict,
                    ErrorMessage = "Username is already in use."
                });

            var user = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded)
                return BadRequest(new Error
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = "User could not be created, try again later."
                });
            
            // If this is the first user to be added, make them an admin.
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.UsernameOrEmail) ?? await _userManager.FindByEmailAsync(model.UsernameOrEmail);

            if (user == null)
                return NotFound(new Error
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = "Username could not be found."
                });
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized(new Error
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    ErrorMessage = "The entered password was incorrect."
                });

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Issuer = _configuration["AppSettings:JwtSettings:Issuer"],
                Audience = _configuration["AppSettings:JwtSettings:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:JwtSettings:SecretKey"])), SecurityAlgorithms.HmacSha512Signature)
            };

            // Add all of the user's roles to the claims.
            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = await RefreshToken(user);

            return Ok(new LoginResponse
            {
                UserId = user.Id,
                AccessToken = tokenHandler.WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = refreshToken
            });
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound("Invalid UserID");

            var isValid = await _userManager.VerifyUserTokenAsync(user, "Showtime.Auth", "RefreshToken", model.RefreshToken);
            if (!isValid)
                return BadRequest("Refresh token is not valid.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Issuer = _configuration["AppSettings:JwtSettings:Issuer"],
                Audience = _configuration["AppSettings:JwtSettings:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:JwtSettings:SecretKey"])), SecurityAlgorithms.HmacSha512Signature)
            };

            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var newAccessToken = tokenHandler.CreateToken(tokenDescriptor);

            var newRefreshToken = await RefreshToken(user);

            return Ok(new RefreshResponse
            {
                UserId = user.Id,
                NewAccessToken = tokenHandler.WriteToken(newAccessToken),
                NewRefreshToken = newRefreshToken
            });
        }

        private async Task<string> RefreshToken(IdentityUser user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, "Showtime.Auth", "RefreshToken");
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "Showtime.Auth", "RefreshToken");
            await _userManager.SetAuthenticationTokenAsync(user, "Showtime.Auth", "RefreshToken", newRefreshToken);

            return newRefreshToken;
        }
    }
}
