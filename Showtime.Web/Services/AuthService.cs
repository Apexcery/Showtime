using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

using Showtime.Lib.Models.Auth;

namespace Showtime.Web.Services
{
    public interface IAuthService
    {
        Task<IActionResult> Register(RegisterModel model);
        Task<HttpResponseMessage> Login(LoginModel model);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;

        public AuthService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Register(RegisterModel model)
        {
            var response = await _client.PostAsJsonAsync(@"/auth/register", model);
            if (!response.IsSuccessStatusCode)
                return new BadRequestObjectResult(response);

            var registerResponse = await response.Content.ReadFromJsonAsync<RegisterResponse>();
            return new OkObjectResult(registerResponse);
        }

        public async Task<HttpResponseMessage> Login(LoginModel model)
        {
            var response = await _client.PostAsJsonAsync(@"/auth/login", model);
            return response;
        }
    }
}
