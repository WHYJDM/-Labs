// Подключаем необходимые пространства имён
using Lab4.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Lab4.Models;

namespace Lab4.Tests
{
    // Класс для модульных тестов
    [TestClass]
    public class FileHandlerTests
    {
        // Пути к временным файлам, используемым в тестах
        private const string TestFile1 = "test1.json";
        private const string TestFile2 = "test2.json";
        private const string MergedFile = "testMerged.json";

        // Тест проверки сериализации и десериализации
        [TestMethod]
        public async Task TestSerializationAndDeserialization()
        {
            // Arrange
            var drinks = FileHandler.GenerateDrinks(); // 20 напитков
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");

            // Act
            await FileHandler.SerializeDrinksAsync(drinks, filePath);
            var deserializedDrinks = await FileHandler.DeserializeDrinksAsync(filePath);

            // Assert
            Assert.AreEqual(20, deserializedDrinks.Count);

            // Clean up
            File.Delete(filePath);
        }


        // Тест слияния двух файлов
        [TestMethod]
        public async Task TestMergeFilesAsync()
        {
            // Генерация тестовых данных
            var drinks = FileHandler.GenerateDrinks();

            // Сериализуем первую половину в первый файл
            await FileHandler.SerializeDrinksAsync(drinks.GetRange(0, 10), TestFile1);

            // Сериализуем вторую половину во второй файл
            await FileHandler.SerializeDrinksAsync(drinks.GetRange(10, 10), TestFile2);

            // Выполняем слияние двух файлов в один
            await FileHandler.MergeFilesAsync(TestFile1, TestFile2, MergedFile);

            // Десериализуем объединённый файл
            var mergedDrinks = await FileHandler.DeserializeDrinksAsync(MergedFile);

            // Проверяем, что итоговый файл содержит 20 записей
            Assert.AreEqual(20, mergedDrinks.Count);
        }
    }
}
