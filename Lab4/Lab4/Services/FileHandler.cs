using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Lab4.Models;

namespace Lab4.Services
{
    public static class FileHandler
    {
        // Semaphore to ensure thread-safe access to file operations
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Generates a list of 20 drink objects with alternating types and manufacturers.
        /// </summary>
        /// <returns>List of Drink objects</returns>
        public static List<Drink> GenerateDrinks()
        {
            var drinks = new List<Drink>();
            for (int i = 1; i <= 20; i++)
            {
                drinks.Add(new Drink
                {
                    Id = i,
                    Name = $"Drink{i}",
                    SerialNumber = $"SN{i:0000}",
                    Type = i % 2 == 0 ? "Carbonated" : "Non-carbonated",
                    Manufacturer = new Manufacturer
                    {
                        Name = $"Manufacturer{i}",
                        Address = $"Address{i}",
                        IsAChildCompany = i % 2 == 0
                    }
                });
            }
            return drinks;
        }

        /// <summary>
        /// Asynchronously serializes a list of drinks to a JSON file.
        /// </summary>
        /// <param name="drinks">The list of drinks to serialize.</param>
        /// <param name="filePath">Path to the destination file.</param>
        public static async Task SerializeDrinksAsync(List<Drink> drinks, string filePath)
        {
            await semaphore.WaitAsync();
            try
            {
                using FileStream stream = new FileStream(
                    filePath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None); // No other process can read/write while the stream is active

                await JsonSerializer.SerializeAsync(stream, drinks, new JsonSerializerOptions { WriteIndented = true });
                await stream.FlushAsync(); // Ensure all data is written to disk
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Asynchronously deserializes a list of drinks from a JSON file.
        /// </summary>
        /// <param name="filePath">Path to the source file.</param>
        /// <returns>Deserialized list of Drink objects.</returns>
        public static async Task<List<Drink>> DeserializeDrinksAsync(string filePath)
        {
            await semaphore.WaitAsync();
            try
            {
                using FileStream stream = new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read); // Allow read access for other processes

                return await JsonSerializer.DeserializeAsync<List<Drink>>(stream);
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Merges two JSON files containing drinks into a single merged file.
        /// </summary>
        /// <param name="file1">Path to the first source file.</param>
        /// <param name="file2">Path to the second source file.</param>
        /// <param name="mergedFile">Path to the output merged file.</param>
        public static async Task MergeFilesAsync(string file1, string file2, string mergedFile)
        {
            var task1 = DeserializeDrinksAsync(file1);
            var task2 = DeserializeDrinksAsync(file2);

            await Task.WhenAll(task1, task2);

            var allDrinks = new List<Drink>();
            allDrinks.AddRange(await task1);
            allDrinks.AddRange(await task2);

            await SerializeDrinksAsync(allDrinks, mergedFile);
        }

        /// <summary>
        /// Asynchronously reads and prints all drinks from the merged file using parallel tasks.
        /// </summary>
        /// <param name="mergedFile">Path to the merged JSON file.</param>
        public static async Task PrintMergedFileAsync(string mergedFile)
        {
            var drinks = await DeserializeDrinksAsync(mergedFile);

            var tasks = new List<Task>();
            foreach (var drink in drinks)
            {
                tasks.Add(Task.Run(() =>
                {
                    Console.WriteLine($"{drink.Id}: {drink.Name} ({drink.Type}), Manufacturer: {drink.Manufacturer.Name}");
                }));
            }

            await Task.WhenAll(tasks);
        }
    }
}
