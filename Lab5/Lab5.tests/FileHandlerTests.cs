// Подключаем необходимые пространства имен
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab5.Services;
using System.IO;

namespace LAB5.Tests  // Пространство имен для тестов проекта LAB5
{
    // Атрибут указывает, что этот класс содержит тестовые методы
    [TestClass]
    public class FileHandlerTests
    {
        // Тест проверяет, что метод GenerateDrinks генерирует правильное количество напитков
        [TestMethod]
        public void GenerateDrinks_CreatesCorrectAmount()
        {
            // Создаем экземпляр класса FileHandler
            var handler = new FileHandler();

            // Генерируем список из 50 напитков
            var drinks = handler.GenerateDrinks(50);

            // Проверяем, что действительно создано 50 напитков
            Assert.AreEqual(50, drinks.Count);
        }

        // Тест проверяет корректность сохранения и загрузки данных в/из файла
        [TestMethod]
        public void SaveAndLoadDrinks_FilePersistsCorrectly()
        {
            // Создаем экземпляр обработчика
            var handler = new FileHandler();

            // Генерируем 10 напитков для теста
            var drinks = handler.GenerateDrinks(10);

            // Указываем путь к тестовому файлу
            string path = "test.json";

            // Сохраняем напитки в файл
            handler.SaveDrinksToFile(drinks, path);

            // Загружаем напитки обратно из файла
            var loaded = handler.LoadDrinksFromFile(path);

            // Сравниваем количество объектов до и после, чтобы убедиться, что файл сохранился правильно
            Assert.AreEqual(drinks.Count, loaded.Count);

            // Удаляем файл после теста, чтобы не засорять рабочую директорию
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
