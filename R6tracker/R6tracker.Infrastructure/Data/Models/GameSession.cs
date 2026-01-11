using System;
using System.ComponentModel.DataAnnotations;
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
    }
}
