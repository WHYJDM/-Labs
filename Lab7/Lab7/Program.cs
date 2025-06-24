using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lab7.Data;
using Lab7.Models;
using Lab7.Repositories;
using Lab7.Services;

namespace Lab7
{
    /// <summary>
    /// Console application for managing manufacturers and drinks.
    /// </summary>
    internal static class Program
    {
        private const string ConnectionString = "Data Source=app.db";
        private const string OptionSeed = "1";
        private const string OptionShowManufacturers = "2";
        private const string OptionShowDrinks = "3";
        private const string OptionAddManufacturer = "4";
        private const string OptionAddDrink = "5";
        private const string OptionExit = "6";

        public static async Task Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(ConnectionString)
                .Options;

            await using var context = new AppDbContext(options);
            await context.Database.MigrateAsync();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=== Menu ===");
                Console.WriteLine($"{OptionSeed} - Seed database with data");
                Console.WriteLine($"{OptionShowManufacturers} - Show manufacturers");
                Console.WriteLine($"{OptionShowDrinks} - Show drinks");
                Console.WriteLine($"{OptionAddManufacturer} - Add a manufacturer");
                Console.WriteLine($"{OptionAddDrink} - Add a drink");
                Console.WriteLine($"{OptionExit} - Exit");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case OptionSeed:
                        await DbInitializer.SeedAsync(context);
                        break;
                    case OptionShowManufacturers:
                        await PrintManufacturersAsync(context);
                        break;
                    case OptionShowDrinks:
                        await PrintDrinksAsync(context);
                        break;
                    case OptionAddManufacturer:
                        await AddManufacturerAsync(context);
                        break;
                    case OptionAddDrink:
                        await AddDrinkAsync(context);
                        break;
                    case OptionExit:
                        return;
                    default:
                        Console.WriteLine("Unknown option.");
                        break;
                }
            }
        }

        /// <summary>
        /// Display all manufacturers.
        /// </summary>
        private static async Task PrintManufacturersAsync(AppDbContext context)
        {
            var repo = new ManufacturerRepository(context);
            var list = await repo.GetAllAsync();
            foreach (var m in list)
            {
                Console.WriteLine($"[{m.Id}] {m.Name} ({m.Country})");
            }
        }

        /// <summary>
        /// Display all drinks.
        /// </summary>
        private static async Task PrintDrinksAsync(AppDbContext context)
        {
            var repo = new DrinkRepository(context);
            var list = await repo.GetAllAsync();
            foreach (var d in list)
            {
                Console.WriteLine($"[{d.Id}] {d.Name} (Manufacturer Id={d.ManufacturerId})");
            }
        }

        /// <summary>
        /// Add a new manufacturer.
        /// </summary>
        private static async Task AddManufacturerAsync(AppDbContext context)
        {
            Console.Write("Manufacturer name: ");
            var name = Console.ReadLine() ?? string.Empty;
            Console.Write("Country: ");
            var country = Console.ReadLine() ?? string.Empty;

            var repo = new ManufacturerRepository(context);
            await repo.AddAsync(new Manufacturer { Name = name, Country = country });
            await repo.SaveChangesAsync();

            Console.WriteLine("Manufacturer added.");
        }

        /// <summary>
        /// Add a drink and manufacturer in a single transaction.
        /// </summary>
        private static async Task AddDrinkAsync(AppDbContext context)
        {
            Console.Write("Manufacturer name: ");
            var mName = Console.ReadLine() ?? string.Empty;
            Console.Write("Drink name: ");
            var dName = Console.ReadLine() ?? string.Empty;

            var mRepo = new ManufacturerRepository(context);
            var dRepo = new DrinkRepository(context);
            var svc = new BusinessService(context, mRepo, dRepo);

            try
            {
                await svc.AddDrinkWithManufacturerAsync(mName, dName);
                Console.WriteLine("Drink and manufacturer successfully added.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding: {ex.Message}");
            }
        }
    }
}
