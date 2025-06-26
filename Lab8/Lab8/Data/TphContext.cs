using Microsoft.EntityFrameworkCore;
using Lab8.Models;

namespace Lab8.Data
{
    public class TphContext : DbContext
    {
        private const string ConnectionString = "Data Source=app.db";

        public TphContext() { }
        public TphContext(DbContextOptions<TphContext> options) : base(options) { }

        public DbSet<Drink> Drinks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlite(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Drink>();
            e.ToTable(MappingConstants.TableDrinks);

            e.HasDiscriminator<string>(MappingConstants.DiscriminatorColumn)
             .HasValue<AlcoholicDrink>(MappingConstants.DiscriminatorValueAlcoholic)
             .HasValue<NonAlcoholicDrink>(MappingConstants.DiscriminatorValueNonAlcoholic);
        }
    }
}
