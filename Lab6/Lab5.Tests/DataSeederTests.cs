using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Lab6.Data;

namespace Lab6.Tests
{
    /// <summary>
    /// Unit tests for the DataSeeder class.
    /// </summary>
    [TestClass]
    public class DataSeederTests
    {
        private const string ConnectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Lab6DB;Integrated Security=True";

        /// <summary>
        /// Ensures the seed method creates at least 30 manufacturers in the database.
        /// </summary>
        [TestMethod]
        public async Task SeedAsync_Creates30Manufacturers()
        {
            // Arrange: initialize database schema before seeding data
            DatabaseInitializer.Initialize();

            // Act: seed the database with sample data
            await DataSeeder.SeedAsync();

            // Assert: check that manufacturers were inserted
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            string query = "SELECT COUNT(*) FROM Manufacturers";
            using SqlCommand command = new(query, connection);
            int count = (int)command.ExecuteScalar();

            Assert.IsTrue(count >= 30, "Expected at least 30 manufacturers after seeding.");
        }
    }
}
