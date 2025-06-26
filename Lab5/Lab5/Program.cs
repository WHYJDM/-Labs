// Import models and services from the LAB5 project
using Lab5.Models;
using Lab5.Services;
using System.Collections.Concurrent;

// Create an instance of FileHandler — it contains all the file processing logic
var handler = new FileHandler();

// Create a progress bar — a slash symbol will be displayed after each record is processed
var progress = new Progress<int>(i => Console.Write("/"));

// Start an infinite loop for the console menu
while (true)
{
    // Display the console menu
    Console.WriteLine("\n1 - Generate and save drinks");
    Console.WriteLine("2 - Read files in parallel");
    Console.WriteLine("3 - Exit");

    // Read user input
    var choice = Console.ReadLine();

    // If the user selected option 1
    if (choice == "1")
    {
        // Generate 50 drinks
        var drinks = handler.GenerateDrinks(50);

        // Asynchronously save drinks to 5 separate files
        await handler.SaveToMultipleFilesAsync(drinks);

        // Notify the user of successful save
        Console.WriteLine("Files saved successfully.");
    }
    // If option 2 is selected — read files in parallel
    else if (choice == "2")
    {
        // Create a list of file names: file1.json ... file5.json
        string[] files = Enumerable.Range(1, 5).Select(i => $"file{i}.json").ToArray();

        // Load data from all files in parallel, tracking progress
        var result = await handler.LoadFromFilesAsync(files, progress);

        // Display the reading result from all files
        Console.WriteLine("\n\nReading result:");
        foreach (var pair in result)
        {
            Console.WriteLine($"\nFile: {pair.Key}");
            foreach (var drink in pair.Value)
            {
                Console.WriteLine(drink); // Print each drink
            }
        }

        // Sort all entries in the dictionary by ID
        handler.SortDictionary(result);
        Console.WriteLine("\nDictionary sorted by ID.");
    }
    // If option 3 is selected — exit the program
    else if (choice == "3")
    {
        break;
    }
    // Handle invalid input
    else
    {
        Console.WriteLine("Invalid selection.");
    }
}
