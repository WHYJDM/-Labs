using Lab7.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Lab7.Data
{
    /// <summary>
    /// Заполняет 30 Manufacturers и 30 Drinks.
    /// </summary>
    public static class DbInitializer
    {
        private const int SeedCount = 30;

        public static async Task SeedAsync(AppDbContext ctx)
        {
            await ctx.Database.MigrateAsync();

            if (!ctx.Manufacturers.Any())
            {
                for (int i = 1; i <= SeedCount; i++)
                    ctx.Manufacturers.Add(new Manufacturer { Name = $"Manufacturer {i}" });

                await ctx.SaveChangesAsync();
            }

            if (!ctx.Drinks.Any())
            {
                var ids = ctx.Manufacturers.Select(m => m.Id).ToArray();
                for (int i = 1; i <= SeedCount; i++)
                {
                    ctx.Drinks.Add(new Drink
                    {
                        Name = $"Drink {i}",
                        ManufacturerId = ids[i % ids.Length]
                    });
                }
                await ctx.SaveChangesAsync();
            }
        }
    }
}
