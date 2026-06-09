using System.ComponentModel.DataAnnotations;

namespace R6tracker.Core.DTOs;

public class CreateMapDto
{
    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Location { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}