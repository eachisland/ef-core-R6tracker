using Microsoft.AspNetCore.Identity;
using R6tracker.Infrastructure.Data.Models;

namespace R6tracker.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(
        R6trackerDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Administrator"))
            await roleManager.CreateAsync(new IdentityRole("Administrator"));

        if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(new IdentityRole("User"));

        if (await userManager.FindByEmailAsync("admin@r6tracker.com") == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@r6tracker.com",
                Email = "admin@r6tracker.com",
                EmailConfirmed = true,
                DisplayName = "Admin",
                Country = "BG"
            };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Administrator");
        }
        if (!context.R6Players.Any())
        {
            var adminUser = await userManager.FindByEmailAsync("admin@r6tracker.com");

            if (adminUser != null)
            {
                context.R6Players.AddRange(
                    new R6Player
                    {
                        Id = Guid.NewGuid().ToString(),
                        PlayerName = "ShadowR",
                        Platform = "PC",
                        Level = 120,
                        MatchesPlayed = 340,
                        Kills = 1200,
                        Deaths = 800,
                        KillDeathRatio = 1.50,
                        Country = "BG",
                        UserId = adminUser.Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new R6Player
                    {
                        Id = Guid.NewGuid().ToString(),
                        PlayerName = "NovaSix",
                        Platform = "PS5",
                        Level = 85,
                        MatchesPlayed = 210,
                        Kills = 650,
                        Deaths = 700,
                        KillDeathRatio = 0.93,
                        Country = "US",
                        UserId = adminUser.Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new R6Player
                    {
                        Id = Guid.NewGuid().ToString(),
                        PlayerName = "IceWall",
                        Platform = "Xbox",
                        Level = 200,
                        MatchesPlayed = 500,
                        Kills = 2100,
                        Deaths = 1400,
                        KillDeathRatio = 1.50,
                        Country = "DE",
                        UserId = adminUser.Id,
                        CreatedAt = DateTime.UtcNow
                    }
                );
                await context.SaveChangesAsync();
            }
        }

        if (!context.Maps.Any())
        {
            context.Maps.AddRange(
                new R6Map { Name = "Border", Location = "Mexico", Type = "Bomb", IsActive = true },
                new R6Map { Name = "Clubhouse", Location = "Germany", Type = "Bomb", IsActive = true },
                new R6Map { Name = "Coastline", Location = "Spain", Type = "Bomb", IsActive = true },
                new R6Map { Name = "Consulate", Location = "France", Type = "Bomb", IsActive = true },
                new R6Map { Name = "Kafe", Location = "Russia", Type = "Bomb", IsActive = true },
                new R6Map { Name = "Oregon", Location = "USA", Type = "Bomb", IsActive = true },
                new R6Map { Name = "Villa", Location = "Italy", Type = "Bomb", IsActive = true },
                new R6Map { Name = "Bank", Location = "USA", Type = "Bomb", IsActive = false },
                new R6Map { Name = "Chalet", Location = "France", Type = "Bomb", IsActive = false },
                new R6Map { Name = "Kanal", Location = "Germany", Type = "Bomb", IsActive = false }
            );
            await context.SaveChangesAsync();
        }
    }
}