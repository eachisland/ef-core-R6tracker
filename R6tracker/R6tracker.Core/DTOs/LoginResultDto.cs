namespace R6tracker.Core.DTOs;

public class LoginResultDto
{
    public string Token { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public string PlayerId { get; set; } = string.Empty;
}