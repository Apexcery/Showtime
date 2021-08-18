using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Showtime.Lib.Models.Auth;

namespace Showtime.Web.Services
{
    public interface IAuthService
    {
        Task<HttpResponseMessage> Register(RegisterModel model);
        Task<HttpResponseMessage> Login(LoginModel model);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;

        public AuthService(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> Register(RegisterModel model)
        {
            var response = await _client.PostAsJsonAsync(@"auth/register", model);
            return response;
        }

        public async Task<HttpResponseMessage> Login(LoginModel model)
        {
            var response = await _client.PostAsJsonAsync(@"auth/login", model);
            return response;
        }
    }
}
