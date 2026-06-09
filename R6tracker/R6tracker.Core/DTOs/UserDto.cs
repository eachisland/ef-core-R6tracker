namespace R6tracker.Core.DTOs;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime RegisteredOn { get; set; }
    public string Role { get; set; } = string.Empty;
}