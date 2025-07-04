﻿using Lab3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using System.Diagnostics;

namespace LAB3
{
    class Program
    {
        static string file1 = "drinks1.xml";
        static string file2 = "drinks2.xml";
        static string mergedFile = "merged_drinks.txt";

        static void Main()
        {
            var drinks = GenerateDrinks();
            var firstTen = drinks.Take(10).ToList();
            var secondTen = drinks.Skip(10).Take(10).ToList();

            var serializer = new XmlSerializer(typeof(List<Drink>));

            Thread t1 = new Thread(() => SerializeToFile(firstTen, file1, serializer));
            Thread t2 = new Thread(() => SerializeToFile(secondTen, file2, serializer));

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("Task 1 completed. Files serialized.");

            Thread mergeThread = new Thread(() => MergeFiles(file1, file2, mergedFile));
            mergeThread.Start();
            mergeThread.Join();

            Console.WriteLine("Task 2 completed. Files merged into one.");

            RunReadingTasks();
        }

        static List<Drink> GenerateDrinks()
        {
            var list = new List<Drink>();
            for (int i = 1; i <= 20; i++)
            {
                list.Add(Drink.Create(
                    i,
                    $"Drink-{i}",
                    $"SN{i:00000}",
                    i % 2 == 0 ? "Soft" : "Hard",
                    Manufacturer.Create($"Manuf-{i}", $"Address-{i}", false)));
            }
            return list;
        }

        static void SerializeToFile(List<Drink> drinks, string fileName, XmlSerializer serializer)
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(stream, drinks);
            }
        }

        static void MergeFiles(string file1, string file2, string mergedFile)
        {
            var serializer = new XmlSerializer(typeof(List<Drink>));
            var list1 = ReadFromFile(file1, serializer);
            var list2 = ReadFromFile(file2, serializer);

            using (StreamWriter writer = new StreamWriter(mergedFile))
            {
                int count = Math.Min(list1.Count, list2.Count);
                for (int i = 0; i < count; i++)
                {
                    writer.WriteLine($"{list1[i].Name} - {list1[i].SerialNumber} - {list1[i].DrinkType} - {list1[i].Manufacturer.Name}");
                    writer.WriteLine($"{list2[i].Name} - {list2[i].SerialNumber} - {list2[i].DrinkType} - {list2[i].Manufacturer.Name}");
                }
            }
        }

        static List<Drink> ReadFromFile(string fileName, XmlSerializer serializer)
        {
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                return (List<Drink>)serializer.Deserialize(stream);
            }
        }

        static void RunReadingTasks()
        {
            Console.WriteLine("\nTask 3.1 — Reading in a single thread:");
            var sw = Stopwatch.StartNew();
            var allLines = File.ReadAllLines(mergedFile);
            foreach (var line in allLines)
                Console.WriteLine(line);
            Console.Out.Flush();
            sw.Stop();
            Console.WriteLine($"Time: {sw.Elapsed.TotalMilliseconds:F3} ms\n");

            Console.WriteLine("Task 3.2 — Reading in two threads:");
            var lines = File.ReadAllLines(mergedFile);
            var half = lines.Length / 2;
            sw.Restart();

            Thread t1 = new Thread(() =>
            {
                ReadLines(lines.Take(half).ToArray());
                Console.Out.Flush();
            });

            Thread t2 = new Thread(() =>
            {
                ReadLines(lines.Skip(half).ToArray());
                Console.Out.Flush();
            });

            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

            sw.Stop();
            Console.WriteLine($"Time: {sw.ElapsedMilliseconds} ms\n");

            Console.WriteLine("Task 3.3 — 10 threads with semaphore:");
            SemaphoreSlim semaphore = new SemaphoreSlim(5);
            sw.Restart();
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < 10; i++)
            {
                int id = i;
                var thread = new Thread(() =>
                {
                    semaphore.Wait();
                    Console.WriteLine($"Thread {id} started reading...");
                    foreach (var line in lines)
                        Console.WriteLine($"[{id}] {line}");
                    Console.Out.Flush();
                    semaphore.Release();
                });
                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
                thread.Join();

            sw.Stop();
            Console.WriteLine($"Time: {sw.ElapsedMilliseconds} ms\n");
        }

        static void ReadLines(string[] lines)
        {
            foreach (var line in lines)
                Console.WriteLine(line);
        }
    }
}
