using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using R6tracker.Core.Interfaces;
using R6tracker.Infrastructure.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace R6tracker.Core.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly IConfiguration config;
    private readonly ILogger<AuthService> logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration config,
        ILogger<AuthService> logger)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.config = config;
        this.logger = logger;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            logger.LogWarning("Login failed - user {Email} not found", email);
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);

        if (!result.Succeeded)
        {
            logger.LogWarning("Login failed - wrong password for {Email}", email);
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var roles = await userManager.GetRolesAsync(user);
        logger.LogInformation("User {Email} logged in", email);
        return GenerateToken(user, roles);
    }

    public async Task RegisterAsync(string email, string password, string displayName, string country)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            DisplayName = displayName,
            Country = country,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogWarning("Registration failed for {Email}: {Errors}", email, errors);
            throw new InvalidOperationException(errors);
        }

        await userManager.AddToRoleAsync(user, "User");
        logger.LogInformation("User {Email} registered", email);
    }

    private string GenerateToken(ApplicationUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.DisplayName)
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}