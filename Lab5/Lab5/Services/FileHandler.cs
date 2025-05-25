using System.Collections.Concurrent;
using Lab5.Models;
using System.Text.Json;

namespace Lab5.Services;

public class FileHandler
{
    private readonly object _lock = new();

    public List<Drink> GenerateDrinks(int count)
    {
        var drinks = new List<Drink>();
        for (int i = 1; i <= count; i++)
        {
            drinks.Add(new Drink
            {
                Id = i,
                Name = $"Drink {i}",
                Manufacturer = new Manufacturer
                {
                    Name = $"Maker {i % 5 + 1}",
                    Country = $"Country {i % 3 + 1}"
                }
            });
        }
        return drinks;
    }

    public void SaveDrinksToFile(List<Drink> drinks, string filePath)
    {
        var json = JsonSerializer.Serialize(drinks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Drink> LoadDrinksFromFile(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Drink>>(json) ?? new List<Drink>();
    }

    public async Task SaveToMultipleFilesAsync(List<Drink> drinks)
    {
        var tasks = new List<Task>();
        int fileCount = 5;
        int perFile = drinks.Count / fileCount;

        for (int i = 0; i < fileCount; i++)
        {
            int start = i * perFile;
            var chunk = drinks.Skip(start).Take(perFile).ToList();
            string fileName = $"file{i + 1}.json";

            tasks.Add(Task.Run(() =>
            {
                lock (_lock)
                {
                    SaveDrinksToFile(chunk, fileName);
                }
            }));
        }

        await Task.WhenAll(tasks);
    }

    public async Task<ConcurrentDictionary<string, ConcurrentBag<Drink>>> LoadFromFilesAsync(string[] filePaths, IProgress<int> progress)
    {
        var dict = new ConcurrentDictionary<string, ConcurrentBag<Drink>>();
        var tasks = new List<Task>();

        foreach (var path in filePaths)
        {
            tasks.Add(Task.Run(() =>
            {
                var drinks = LoadDrinksFromFile(path);
                var bag = new ConcurrentBag<Drink>(drinks);

                dict[path] = bag;

                foreach (var _ in drinks)
                {
                    progress.Report(1);
                    Thread.Sleep(100); // для демонстрации прогресса
                }
            }));
        }

        await Task.WhenAll(tasks);
        return dict;
    }

    public void SortDictionary(ConcurrentDictionary<string, ConcurrentBag<Drink>> dict)
    {
        foreach (var key in dict.Keys)
        {
            var sorted = dict[key].OrderBy(d => d.Id).ToList();
            dict[key] = new ConcurrentBag<Drink>(sorted);
        }
    }
}
