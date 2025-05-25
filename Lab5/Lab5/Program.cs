// Подключаем модели и сервисы из проекта LAB5
using Lab5.Models;
using Lab5.Services;
using System.Collections.Concurrent;

// Создаем экземпляр класса FileHandler — он содержит всю логику обработки файлов
var handler = new FileHandler();

// Создаем прогрессбар — после каждой записи будет выводиться символ \
var progress = new Progress<int>(i => Console.Write("/"));

// Запускаем бесконечный цикл для консольного меню
while (true)
{
    // Выводим консольное меню
    Console.WriteLine("\n1 - Сгенерировать и сохранить напитки");
    Console.WriteLine("2 - Прочитать файлы параллельно");
    Console.WriteLine("3 - Выйти");

    // Считываем выбор пользователя
    var choice = Console.ReadLine();

    // Если пользователь выбрал пункт 1
    if (choice == "1")
    {
        // Генерируем 50 напитков
        var drinks = handler.GenerateDrinks(50);

        // Асинхронно сохраняем напитки в 5 отдельных файлов
        await handler.SaveToMultipleFilesAsync(drinks);

        // Уведомляем пользователя об успешной записи
        Console.WriteLine("Файлы успешно записаны.");
    }
    // Если выбран пункт 2 — читаем файлы параллельно
    else if (choice == "2")
    {
        // Создаем список имен файлов file1.json ... file5.json
        string[] files = Enumerable.Range(1, 5).Select(i => $"file{i}.json").ToArray();

        // Загружаем данные из всех файлов параллельно, отслеживая прогресс
        var result = await handler.LoadFromFilesAsync(files, progress);

        // Выводим результат чтения из всех файлов
        Console.WriteLine("\n\nРезультат чтения:");
        foreach (var pair in result)
        {
            Console.WriteLine($"\nФайл: {pair.Key}");
            foreach (var drink in pair.Value)
            {
                Console.WriteLine(drink); // Печатаем каждый напиток
            }
        }

        // Сортируем все записи в словаре по ID
        handler.SortDictionary(result);
        Console.WriteLine("\nСловарь отсортирован по ID.");
    }
    // Если выбран пункт 3 — выходим из программы
    else if (choice == "3")
    {
        break;
    }
    // Обработка неверного ввода
    else
    {
        Console.WriteLine("Неверный выбор.");
    }
}
