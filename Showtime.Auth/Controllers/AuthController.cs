using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
                return NotFound("User could not be found.");
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return BadRequest("Password is incorrect.");

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
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:JwtSettings:SecretKey"])), SecurityAlgorithms.HmacSha512Signature)
            };

            // Add all of the user's roles to the claims.
            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new LoginResponse
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = token.ValidTo,
                Id = user.Id
            });
        }
    }
}
