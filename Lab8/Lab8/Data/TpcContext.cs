using Microsoft.EntityFrameworkCore;
using Lab8.Models;

namespace Lab8.Data
{
    public class TpcContext : DbContext
    {
        private const string ConnectionString = "Data Source=app.db";

        public TpcContext() { }
        public TpcContext(DbContextOptions<TpcContext> options) : base(options) { }

        public DbSet<AlcoholicDrink> AlcoholicDrinks { get; set; } = null!;
        public DbSet<NonAlcoholicDrink> NonAlcoholicDrinks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlite(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlcoholicDrink>()
                        .ToTable(MappingConstants.TableAlcoholicDrinks);

            modelBuilder.Entity<NonAlcoholicDrink>()
                        .ToTable(MappingConstants.TableNonAlcoholicDrinks);

            modelBuilder.Ignore<Drink>();
        }
    }
}
