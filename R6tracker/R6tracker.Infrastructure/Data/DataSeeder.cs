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
    }
}