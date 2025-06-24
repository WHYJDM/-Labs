using Lab7.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab7.Data
{
    /// <summary>
    /// EF Core DbContext для сущностей Manufacturer и Drink.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; } = null!;
        public DbSet<Drink> Drinks { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Автоматически применяем все IEntityTypeConfiguration из сборки
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
