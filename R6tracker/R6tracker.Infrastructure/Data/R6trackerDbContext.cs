using R6tracker.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace R6tracker.Infrastructure.Data
{
    public class R6trackerDbContext : DbContext
    {
        //! REMOVE CONNECTION STRING DETAILS
         private const string ConnectionString =
	        "";

        public DbSet<R6Player> R6Players { get; set; }  
        public DbSet<GameSession> GameSessions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder.UseSqlServer(ConnectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}