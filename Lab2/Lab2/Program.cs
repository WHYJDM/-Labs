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
                    Console.WriteLine("\nMenu:");
                    Console.WriteLine("1. Create 10 drinks");
                    Console.WriteLine("2. Serialize to XML");
                    Console.WriteLine("3. Show XML file content");
                    Console.WriteLine("4. Deserialize from XML and display");
                    Console.WriteLine("5. Find all Models (by SerialNumber, using XDocument)");
                    Console.WriteLine("6. Find all Models (by SerialNumber, using XmlDocument)");
                    Console.WriteLine("7. Update SerialNumber value (XDocument)");
                    Console.WriteLine("8. Update SerialNumber value (XmlDocument)");
                    Console.WriteLine("0. Exit");
                    Console.Write("Choice: ");

                    string choice = Console.ReadLine() ?? "";
                    Console.Clear();

                    switch (choice)
                    {
                        case "1":
                            drinks = CreateDrinks();
                            Console.WriteLine("10 objects created.");
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
                            Console.WriteLine("Invalid input.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private static List<Drink> CreateDrinks()
        {
            var list = new List<Drink>();
            for (int i = 1; i <= 10; i++)
            {
                var manufacturer = new Manufacturer
                {
                    Name = $"Factory #{i}",
                    Address = $"City #{i}"
                };
                var drink = new Drink
                {
                    Name = $"Drink {i}",
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
                Console.WriteLine($"Serialization completed. File: {FileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Serialization error: {ex.Message}");
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
                Console.WriteLine($"Error reading XML: {ex.Message}");
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

            Console.Write("Enter element number (0-9): ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 0 || index >= drinks.Count)
            {
                Console.WriteLine("Invalid index.");
                return;
            }

            Console.Write("Enter new SerialNumber value: ");
            string newValue = Console.ReadLine();

            var serialElem = drinks[index].Element("SerialNumber");
            if (serialElem != null)
            {
                serialElem.Value = newValue;
                doc.Save(FileName);
                Console.WriteLine("SerialNumber updated.");
            }
            else
            {
                Console.WriteLine("SerialNumber not found.");
            }
        }

        private static void UpdateAttributeXmlDocument()
        {
            var doc = new XmlDocument();
            doc.Load(FileName);
            var nodes = doc.GetElementsByTagName("Drink");

            Console.Write("Enter element number (0-9): ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 0 || index >= nodes.Count)
            {
                Console.WriteLine("Invalid index.");
                return;
            }

            Console.Write("Enter new SerialNumber value: ");
            string newValue = Console.ReadLine();

            var node = nodes[index];
            var serialNode = node.SelectSingleNode("SerialNumber");
            if (serialNode != null)
            {
                serialNode.InnerText = newValue;
                doc.Save(FileName);
                Console.WriteLine("SerialNumber updated.");
            }
            else
            {
                Console.WriteLine("SerialNumber not found.");
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
            Console.WriteLine($"Name: {Name}, SN: {SerialNumber}, Type: {DrinkType}, Manufacturer: {Manufacturer?.Name}, Address: {Manufacturer?.Address}");
        }
    }

    [Serializable]
    public class Manufacturer
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
