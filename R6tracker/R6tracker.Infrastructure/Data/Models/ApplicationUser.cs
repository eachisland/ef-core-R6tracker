using Microsoft.AspNetCore.Identity;

namespace R6tracker.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public DateTime RegisteredOn { get; set; } = DateTime.UtcNow;
        public string? Country { get; set; }

        public ICollection<R6Player> Players { get; set; } = new List<R6Player>();
    }
}