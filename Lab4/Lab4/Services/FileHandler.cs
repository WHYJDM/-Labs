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
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

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
                    Type = i % 2 == 0 ? "Газированный" : "Без газа",
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

        public static async Task SerializeDrinksAsync(List<Drink> drinks, string filePath)
        {
            await semaphore.WaitAsync();
            try
            {
                using FileStream stream = new FileStream(
                    filePath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None); // НИКТО не может читать/писать пока поток активен

                await JsonSerializer.SerializeAsync(stream, drinks, new JsonSerializerOptions { WriteIndented = true });
                await stream.FlushAsync(); // Обязательно!
            }
            finally
            {
                semaphore.Release();
            }
        }

        public static async Task<List<Drink>> DeserializeDrinksAsync(string filePath)
        {
            await semaphore.WaitAsync();
            try
            {
                using FileStream stream = new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read); // Разрешаем только чтение другим

                return await JsonSerializer.DeserializeAsync<List<Drink>>(stream);
            }
            finally
            {
                semaphore.Release();
            }
        }

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

        public static async Task PrintMergedFileAsync(string mergedFile)
        {
            var drinks = await DeserializeDrinksAsync(mergedFile);

            var tasks = new List<Task>();
            foreach (var drink in drinks)
            {
                tasks.Add(Task.Run(() =>
                {
                    Console.WriteLine($"{drink.Id}: {drink.Name} ({drink.Type}), Производитель: {drink.Manufacturer.Name}");
                }));
            }

            await Task.WhenAll(tasks);
        }
    }
}
