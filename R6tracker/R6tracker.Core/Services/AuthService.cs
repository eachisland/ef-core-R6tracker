using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using R6tracker.Core.DTOs;
using R6tracker.Core.Interfaces;
using R6tracker.Infrastructure.Data;
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
    private readonly R6trackerDbContext context;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration config,
        ILogger<AuthService> logger,
        R6trackerDbContext context)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.config = config;
        this.logger = logger;
        this.context = context;
    }

    public async Task<LoginResultDto> LoginAsync(string email, string password)
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
        var token = GenerateToken(user, roles);
        var player = context.R6Players.FirstOrDefault(p => p.UserId == user.Id);

        logger.LogInformation("User {Email} logged in", email);

        return new LoginResultDto
        {
            Token = token,
            DisplayName = user.DisplayName,
            IsAdmin = roles.Contains("Administrator"),
            PlayerId = player != null ? player.Id : string.Empty
        };
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

        var player = new R6Player
        {
            Id = Guid.NewGuid().ToString(),
            PlayerName = displayName,
            Platform = "PC",
            Level = 1,
            MatchesPlayed = 0,
            Kills = 0,
            Deaths = 0,
            KillDeathRatio = 0,
            Country = country,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        context.R6Players.Add(player);
        await context.SaveChangesAsync();

        logger.LogInformation("User {Email} registered with player profile", email);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = userManager.Users.ToList();
        var result = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            result.Add(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Country = user.Country,
                RegisteredOn = user.RegisteredOn,
                Role = roles.FirstOrDefault() ?? "User"
            });
        }

        return result;
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