using System.ComponentModel.DataAnnotations;

namespace R6tracker.Core.DTOs;

public class CreatePlayerDto
{
    [Required]
    [MaxLength(30)]
    public string PlayerName { get; set; } = string.Empty;

    [Required]
    public string Platform { get; set; } = string.Empty;

    [Range(1, 500)]
    public int Level { get; set; }

    [Range(0, int.MaxValue)]
    public int MatchesPlayed { get; set; }

    [Range(0, int.MaxValue)]
    public int Kills { get; set; }

    [Range(0, int.MaxValue)]
    public int Deaths { get; set; }

    [Required]
    [MaxLength(50)]
    public string Country { get; set; } = string.Empty;
}