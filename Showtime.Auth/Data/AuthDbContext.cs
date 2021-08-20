using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Showtime.Auth.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        private readonly IConfiguration _configuration;

        public AuthDbContext(IConfiguration configuration, DbContextOptions<AuthDbContext> options) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("ShowtimeConnectionStringPublic"),
                x => x.MigrationsHistoryTable("__MigrationHistory", "Auth"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Setting the schema so that the tables are separated in the database.
            builder.HasDefaultSchema("Auth");

            // Removing columns that won't be used, can be changed later if decision changes.
            builder.Entity<IdentityUser>()
                .Ignore(x => x.EmailConfirmed)
                .Ignore(x => x.PhoneNumber)
                .Ignore(x => x.PhoneNumberConfirmed)
                .Ignore(x => x.TwoFactorEnabled);

            // Renaming tables to be more user friendly.
            builder.Entity<IdentityUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        }
    }
}
