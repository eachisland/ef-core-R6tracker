using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R6tracker.Infrastructure.Data.Models;

namespace R6tracker.Infrastructure.Data.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<GameSession>
    {
        public void Configure(EntityTypeBuilder<GameSession> builder)
        {
            builder.HasKey(g => g.Id);
        }
    }
}
