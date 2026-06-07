using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using R6tracker.Infrastructure.Data.Models;

namespace R6tracker.Infrastructure.Data
{
    public class R6trackerDbContext : IdentityDbContext<ApplicationUser>
    {
        public R6trackerDbContext(DbContextOptions<R6trackerDbContext> options)
            : base(options)
        {
        }

        public DbSet<R6Player> R6Players { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Rank> Ranks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<Rank>().HasData(
                new Rank { Id = "rank-copper",  Name = "Copper",   Tier = 1 },
                new Rank { Id = "rank-bronze",  Name = "Bronze",   Tier = 2 },
                new Rank { Id = "rank-silver",  Name = "Silver",   Tier = 3 },
                new Rank { Id = "rank-gold",    Name = "Gold",     Tier = 4 },
                new Rank { Id = "rank-plat",    Name = "Platinum", Tier = 5 },
                new Rank { Id = "rank-diamond", Name = "Diamond",  Tier = 6 },
                new Rank { Id = "rank-champ",   Name = "Champion", Tier = 7 }
            );

            var adminRoleId = "role-admin-id";
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId,    Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
                new IdentityRole { Id = "role-user-id", Name = "User",          NormalizedName = "USER" }
            );

            var adminUserId = "admin-user-id";
            var hasher = new PasswordHasher<ApplicationUser>();
            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@r6tracker.com",
                NormalizedUserName = "ADMIN@R6TRACKER.COM",
                Email = "admin@r6tracker.com",
                NormalizedEmail = "ADMIN@R6TRACKER.COM",
                EmailConfirmed = true,
                DisplayName = "Admin",
                Country = "BG",
                RegisteredOn = new DateTime(2024, 1, 1)
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");
            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = adminUserId, RoleId = adminRoleId }
            );
        }
    }
}