using System;
using System.Threading.Tasks;
using Lab4.Services;

namespace Lab4
{
    class Program
    {
        // The main entry point of the program uses an asynchronous Main method.
        static async Task Main(string[] args)
        {
            // File paths used by the program
            string path1 = "file1.json";
            string path2 = "file2.json";
            string merged = "merged.json";

            while (true)
            {
                // Display the console menu
                Console.WriteLine("1. Serialize into 2 files");
                Console.WriteLine("2. Merge the files into a third one");
                Console.WriteLine("3. Asynchronously read the merged file");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        var drinks = FileHandler.GenerateDrinks();
                        var task1 = FileHandler.SerializeDrinksAsync(drinks.GetRange(0, 10), path1);
                        var task2 = FileHandler.SerializeDrinksAsync(drinks.GetRange(10, 10), path2);
                        await Task.WhenAll(task1, task2);
                        Console.WriteLine("Files have been written.");
                        break;

                    case "2":
                        await FileHandler.MergeFilesAsync(path1, path2, merged);
                        Console.WriteLine("Files have been merged.");
                        break;

                    case "3":
                        await FileHandler.PrintMergedFileAsync(merged);
                        break;

                    case "0":
                        Console.WriteLine("Exiting program...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}
