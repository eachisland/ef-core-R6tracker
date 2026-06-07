using System.ComponentModel.DataAnnotations;

namespace R6tracker.Core.DTOs;

public class CreateGameSessionDto
{
    [Required]
    public string PlayerId { get; set; } = string.Empty;

    [Required]
    public DateTime DatePlayed { get; set; } = DateTime.UtcNow;

    [Range(0, int.MaxValue)]
    public int Kills { get; set; }

    [Range(0, int.MaxValue)]
    public int Deaths { get; set; }

    [Required]
    [MaxLength(10)]
    public string Result { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Map { get; set; }
}