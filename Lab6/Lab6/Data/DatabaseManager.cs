using System;
using System.Data.SqlClient;

namespace Lab6.Data;

public static class DatabaseManager
{
    private const string ConnectionString =
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Lab6DB;Integrated Security=True";
    public static void AddManufacturer()
    {
        Console.Write("Enter manufacturer name: ");
        string? name = Console.ReadLine();

        Console.Write("Enter country: ");
        string? country = Console.ReadLine();

        using SqlConnection connection = new(ConnectionString);
        connection.Open();

        string query = "INSERT INTO Manufacturers (Name, Country) VALUES (@name, @country)";
        using SqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@country", country);

        command.ExecuteNonQuery();
        Console.WriteLine("Manufacturer added successfully.");
    }
    public static void AddDrink()
    {
        Console.Write("Enter drink name: ");
        string? name = Console.ReadLine();

        Console.Write("Enter manufacturer ID: ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int manufacturerId))
        {
            Console.WriteLine("Invalid manufacturer ID.");
            return;
        }

        using SqlConnection connection = new(ConnectionString);
        connection.Open();

        string query = "INSERT INTO Drinks (Name, ManufacturerId) VALUES (@name, @manufacturerId)";
        using SqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@manufacturerId", manufacturerId);

        command.ExecuteNonQuery();
        Console.WriteLine("Drink added successfully.");
    }
    public static void ShowDrinksByManufacturer()
    {
        Console.Write("Enter manufacturer ID: ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int manufacturerId))
        {
            Console.WriteLine("Invalid manufacturer ID.");
            return;
        }

        using SqlConnection connection = new(ConnectionString);
        connection.Open();

        string query = "SELECT Name FROM Drinks WHERE ManufacturerId = @manufacturerId";
        using SqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@manufacturerId", manufacturerId);

        using SqlDataReader reader = command.ExecuteReader();
        Console.WriteLine("Drinks for this manufacturer:");
        while (reader.Read())
        {
            Console.WriteLine($"- {reader.GetString(0)}");
        }
    }
    public static void ShowAllManufacturers()
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();

        string query = "SELECT Id, Name, Country FROM Manufacturers";
        using SqlCommand command = new(query, connection);
        using SqlDataReader reader = command.ExecuteReader();

        Console.WriteLine("Manufacturers:");
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string country = reader.GetString(2);

            Console.WriteLine($"[{id}] {name} ({country})");
        }
    }
}
