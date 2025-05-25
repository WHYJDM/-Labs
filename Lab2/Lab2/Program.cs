using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;

namespace LAB2
{
    internal class Program
    {
        private const string FileName = "drinks.xml";

        static void Main()
        {
            try
            {
                List<Drink> drinks = new List<Drink>();

                while (true)
                {
                    Console.WriteLine("\nМеню:");
                    Console.WriteLine("1. Создать 10 напитков");
                    Console.WriteLine("2. Сериализовать в XML");
                    Console.WriteLine("3. Показать содержимое XML-файла");
                    Console.WriteLine("4. Десериализовать из XML и вывести");
                    Console.WriteLine("5. Найти все Model (по SerialNumber, XDocument)");
                    Console.WriteLine("6. Найти все Model (по SerialNumber, XmlDocument)");
                    Console.WriteLine("7. Изменить значение SerialNumber (XDocument)");
                    Console.WriteLine("8. Изменить значение SerialNumber (XmlDocument)");
                    Console.WriteLine("0. Выход");
                    Console.Write("Выбор: ");

                    string choice = Console.ReadLine();
                    Console.Clear();

                    switch (choice)
                    {
                        case "1":
                            drinks = CreateDrinks();
                            Console.WriteLine("Создано 10 объектов.");
                            break;
                        case "2":
                            SerializeToXml(drinks);
                            break;
                        case "3":
                            Console.WriteLine(File.ReadAllText(FileName));
                            break;
                        case "4":
                            DeserializeFromXml();
                            break;
                        case "5":
                            FindModelWithXDocument();
                            break;
                        case "6":
                            FindModelWithXmlDocument();
                            break;
                        case "7":
                            UpdateAttributeXDocument();
                            break;
                        case "8":
                            UpdateAttributeXmlDocument();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Неверный ввод.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        private static List<Drink> CreateDrinks()
        {
            var list = new List<Drink>();
            for (int i = 1; i <= 10; i++)
            {
                var manufacturer = new Manufacturer
                {
                    Name = $"Завод #{i}",
                    Address = $"Город #{i}"
                };
                var drink = new Drink
                {
                    Name = $"Напиток {i}",
                    SerialNumber = $"SN{i:0000}",
                    DrinkType = "Soft",
                    Manufacturer = manufacturer
                };
                list.Add(drink);
            }
            return list;
        }

        private static void SerializeToXml(List<Drink> drinks)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<Drink>));
                using var fs = new FileStream(FileName, FileMode.Create);
                serializer.Serialize(fs, drinks);
                Console.WriteLine($"Сериализация завершена. Файл: {FileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сериализации: {ex.Message}");
            }
        }

        private static void DeserializeFromXml()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<Drink>));
                using var fs = new FileStream(FileName, FileMode.Open);
                var drinks = (List<Drink>)serializer.Deserialize(fs);
                foreach (var drink in drinks)
                {
                    drink.Print();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении XML: {ex.Message}");
            }
        }

        private static void FindModelWithXDocument()
        {
            var doc = XDocument.Load(FileName);
            foreach (var elem in doc.Descendants("Drink"))
            {
                var serialNumber = elem.Element("SerialNumber")?.Value;
                if (serialNumber != null)
                    Console.WriteLine($"Model: {serialNumber}");
            }
        }

        private static void FindModelWithXmlDocument()
        {
            var doc = new XmlDocument();
            doc.Load(FileName);
            var nodes = doc.GetElementsByTagName("Drink");
            foreach (XmlNode node in nodes)
            {
                var serialNode = node.SelectSingleNode("SerialNumber");
                if (serialNode != null)
                    Console.WriteLine($"Model: {serialNode.InnerText}");
            }
        }

        private static void UpdateAttributeXDocument()
        {
            var doc = XDocument.Load(FileName);
            var drinks = doc.Descendants("Drink").ToList();

            Console.Write("Введите номер элемента (0-9): ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 0 || index >= drinks.Count)
            {
                Console.WriteLine("Неверный индекс.");
                return;
            }

            Console.Write("Введите новое значение SerialNumber: ");
            string newValue = Console.ReadLine();

            var serialElem = drinks[index].Element("SerialNumber");
            if (serialElem != null)
            {
                serialElem.Value = newValue;
                doc.Save(FileName);
                Console.WriteLine("SerialNumber обновлён.");
            }
            else
            {
                Console.WriteLine("SerialNumber не найден.");
            }
        }

        private static void UpdateAttributeXmlDocument()
        {
            var doc = new XmlDocument();
            doc.Load(FileName);
            var nodes = doc.GetElementsByTagName("Drink");

            Console.Write("Введите номер элемента (0-9): ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 0 || index >= nodes.Count)
            {
                Console.WriteLine("Неверный индекс.");
                return;
            }

            Console.Write("Введите новое значение SerialNumber: ");
            string newValue = Console.ReadLine();

            var node = nodes[index];
            var serialNode = node.SelectSingleNode("SerialNumber");
            if (serialNode != null)
            {
                serialNode.InnerText = newValue;
                doc.Save(FileName);
                Console.WriteLine("SerialNumber обновлён.");
            }
            else
            {
                Console.WriteLine("SerialNumber не найден.");
            }
        }
    }

    [Serializable]
    public class Drink
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string DrinkType { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public void Print()
        {
            Console.WriteLine($"Название: {Name}, SN: {SerialNumber}, Тип: {DrinkType}, Производитель: {Manufacturer?.Name}, Адрес: {Manufacturer?.Address}");
        }
    }

    [Serializable]
    public class Manufacturer
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
