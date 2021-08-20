using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Showtime.Auth.Data
{
    public static class AuthDbInitializer
    {
        // Creating the roles that I want the app to start with.
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.FindByNameAsync("Admin").Result == null)
            {
                var role = new IdentityRole
                {
                    Name = "Admin"
                };

                var _ = roleManager.CreateAsync(role).Result;
            }

            if (roleManager.FindByNameAsync("User").Result == null)
            {
                var role = new IdentityRole
                {
                    Name = "User"
                };

                var _ = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
