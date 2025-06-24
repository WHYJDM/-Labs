// Entry point for the ADO.NET console application
// Following Microsoft C# style conventions and including full comments

using System;
using System.Threading.Tasks;
using Lab6.Data; // Added for database access classes

namespace Lab6_ADO
{
    /// <summary>
    /// Main program class containing the entry point and menu logic.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static async Task Main(string[] args)
        {
            while (true)
            {
                // Display the menu to the user
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1 - Initialize database");
                Console.WriteLine("2 - Seed 30 manufacturers and drinks");
                Console.WriteLine("3 - Add a manufacturer");
                Console.WriteLine("4 - Add a drink");
                Console.WriteLine("5 - Show drinks by manufacturer");
                Console.WriteLine("6 - Exit");
                Console.WriteLine("7 - Show all manufacturers"); // New option

                Console.Write("Enter your choice: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        DatabaseInitializer.Initialize();
                        Console.WriteLine("Database and tables created successfully.");
                        break;

                    case "2":
                        await DataSeeder.SeedAsync();
                        Console.WriteLine("Data seeded successfully.");
                        break;

                    case "3":
                        DatabaseManager.AddManufacturer();
                        break;

                    case "4":
                        DatabaseManager.AddDrink();
                        break;

                    case "5":
                        DatabaseManager.ShowDrinksByManufacturer();
                        break;

                    case "6":
                        return;

                    case "7":
                        DatabaseManager.ShowAllManufacturers(); // New method
                        break;

                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }
            }
        }
    }
}
