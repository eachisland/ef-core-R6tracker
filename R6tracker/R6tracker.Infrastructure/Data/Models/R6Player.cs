using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using R6tracker.Infrastructure.Data.Constants;

namespace R6tracker.Infrastructure.Data.Models
{
    public class R6Player
    {
        public R6Player()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(DBConstants.PlayerConstants.MaxNameLength)]
        public string PlayerName { get; set; } = string.Empty;

        [Required]
        [MaxLength(DBConstants.PlayerConstants.MaxPlatformLength)]
        public string Platform { get; set; } = string.Empty;

        [Range(1, 500)]
        public int Level { get; set; }

        [Range(0, int.MaxValue)]
        public int MatchesPlayed { get; set; }

        [Range(0, int.MaxValue)]
        public int Kills { get; set; }

        [Range(0, int.MaxValue)]
        public int Deaths { get; set; }

        [Range(0.0, 50.0)]
        public double KillDeathRatio { get; set; }

        [Required]
        [MaxLength(DBConstants.PlayerConstants.MaxCountryLength)]
        public string Country { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string? RankId { get; set; }
        [ForeignKey(nameof(RankId))]
        public Rank? Rank { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
        public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();
    }
}