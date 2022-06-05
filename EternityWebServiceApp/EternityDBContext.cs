using EternityWebServiceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EternityWebServiceApp
{
    public class EternityDBContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameScore> GameScores { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
        public DbSet<DataCategory> DataCategories { get; set; }
        public DbSet<ActionCategory> ActionCategories { get; set; }
        public DbSet<DataAction> DataActions { get; set; }
        public DbSet<DataReference> DataReferences { get; set; }

        public EternityDBContext(DbContextOptions<EternityDBContext> options) : base(options) { }

    }
}
