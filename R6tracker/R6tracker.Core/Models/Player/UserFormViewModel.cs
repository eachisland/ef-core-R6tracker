using System.ComponentModel.DataAnnotations;

namespace R6tracker.Core.Models.Player
{
    public class R6PlayerFormViewModel
    {
         [Required(ErrorMessage = "Player name is required.")]
        [StringLength(
            30,
            MinimumLength = 3,
            ErrorMessage = "Player name must be between {2} and {1} characters.")]
        [RegularExpression(
            @"^[A-Za-z0-9_.-]+$",
            ErrorMessage = "Player name contains invalid characters.")]
        public string PlayerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Platform is required.")]
        [StringLength(10)]
        public string Platform { get; set; } = string.Empty;

        [Required]
        [Range(1, 100, ErrorMessage = "Player level must be between 1 and 100.")]
        public int Level { get; set; }

        [Range(0, int.MaxValue)]
        public int MatchesPlayed { get; set; }

        [Range(0, int.MaxValue)]
        public int Kills { get; set; }

        [Range(0, int.MaxValue)]
        public int Deaths { get; set; }

        [Range(0.0, 10.0, ErrorMessage = "K/D must be between 0.0 and 10.0.")]
        public double KillDeathRatio { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(50)]
        public string Country { get; set; } = string.Empty;
    }
}