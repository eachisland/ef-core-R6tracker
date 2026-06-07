using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using R6tracker.Infrastructure.Data.Constants;

namespace R6tracker.Infrastructure.Data.Models
{
    public class GameSession
    {
        public GameSession()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public DateTime DatePlayed { get; set; }

        [Range(0, int.MaxValue)]
        public int Kills { get; set; }

        [Range(0, int.MaxValue)]
        public int Deaths { get; set; }

        [MaxLength(DBConstants.GameConstants.MaxResultLength)]
        public string Result { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Map { get; set; }

        [Required]
        public string PlayerId { get; set; } = string.Empty;
        [ForeignKey(nameof(PlayerId))]
        public R6Player? Player { get; set; }
    }
}