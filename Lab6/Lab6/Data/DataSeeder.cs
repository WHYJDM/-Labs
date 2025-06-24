using Lab6.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lab6.Data;

/// <summary>
/// Responsible for seeding the database with initial data.
/// </summary>
public static class DataSeeder
{
    private const string ConnectionString =
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Lab6DB;Integrated Security=True";

    private static readonly string[] Countries = { "USA", "Germany", "Japan", "France", "Italy" };
    private static readonly string[] DrinkNames = { "Cola", "Lemonade", "Tea", "Juice", "Water" };

    /// <summary>
    /// Seeds the database with 30 manufacturers and corresponding drinks.
    /// </summary>
    public static async Task SeedAsync()
    {
        using SqlConnection connection = new(ConnectionString);
        await connection.OpenAsync();

        for (int i = 1; i <= 30; i++)
        {
            string manufacturerName = $"Manufacturer {i}";
            string country = Countries[i % Countries.Length];

            // Add manufacturer
            int manufacturerId = await InsertManufacturerAsync(connection, manufacturerName, country);

            // Add drink
            string drinkName = $"{DrinkNames[i % DrinkNames.Length]} #{i}";
            await InsertDrinkAsync(connection, drinkName, manufacturerId);
        }
    }

    private static async Task<int> InsertManufacturerAsync(SqlConnection connection, string name, string country)
    {
        string insertQuery = "INSERT INTO Manufacturers (Name, Country) OUTPUT INSERTED.Id VALUES (@name, @country)";
        using SqlCommand command = new(insertQuery, connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@country", country);

        object result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    private static async Task InsertDrinkAsync(SqlConnection connection, string name, int manufacturerId)
    {
        string insertQuery = "INSERT INTO Drinks (Name, ManufacturerId) VALUES (@name, @manufacturerId)";
        using SqlCommand command = new(insertQuery, connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@manufacturerId", manufacturerId);

        await command.ExecuteNonQueryAsync();
    }
}
