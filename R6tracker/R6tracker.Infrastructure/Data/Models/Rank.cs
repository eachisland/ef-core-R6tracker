using System.ComponentModel.DataAnnotations;
using R6tracker.Infrastructure.Data.Constants;

namespace R6tracker.Infrastructure.Data.Models
{
    public class Rank
    {
        public Rank()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(DBConstants.RankConstants.MaxRankNameLength)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 100)]
        public int Tier { get; set; } 
    }
}
