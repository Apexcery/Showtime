using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Showtime.Auth.Services
{
    public interface IUserService
    {
        ClaimsIdentity GetCurrentClaimsIdentity(IIdentity identity);
        Task<IdentityUser> GetCurrentIdentityUser(IIdentity identity);
        Task<IEnumerable<string>> GetCurrentUserRoles(IdentityUser identity);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public ClaimsIdentity GetCurrentClaimsIdentity(IIdentity identity)
        {
            var currentIdentity = identity as ClaimsIdentity;
            return currentIdentity;
        }

        public async Task<IdentityUser> GetCurrentIdentityUser(IIdentity identity)
        {
            var currentIdentity = await _userManager.FindByNameAsync(identity.Name);
            return currentIdentity;
        }

        public async Task<IEnumerable<string>> GetCurrentUserRoles(IdentityUser identity)
        {
            var roles = await _userManager.GetRolesAsync(identity);
            return roles;
        }
    }
}
