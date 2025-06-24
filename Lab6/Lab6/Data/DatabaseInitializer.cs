using Microsoft.Data.SqlClient;
namespace Lab6.Data


{
    public static class DatabaseInitializer
    {
        private const string MasterConnectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

        private const string TargetDatabaseName = "Lab6DB";

        private const string Lab6DbConnectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Lab6DB;Integrated Security=True";

        public static void Initialize()
        {
            using (SqlConnection masterConnection = new(MasterConnectionString))
            {
                masterConnection.Open();

                var checkCommand = new SqlCommand(
                    $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{TargetDatabaseName}') CREATE DATABASE {TargetDatabaseName};",
                    masterConnection);
                checkCommand.ExecuteNonQuery();
            }

            using (SqlConnection connection = new(Lab6DbConnectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    IF OBJECT_ID('Drinks', 'U') IS NOT NULL DROP TABLE Drinks;
                    IF OBJECT_ID('Manufacturers', 'U') IS NOT NULL DROP TABLE Manufacturers;

                    CREATE TABLE Manufacturers (
                        Id INT IDENTITY PRIMARY KEY,
                        Name NVARCHAR(100) NOT NULL,
                        Country NVARCHAR(100) NOT NULL
                    );

                    CREATE TABLE Drinks (
                        Id INT IDENTITY PRIMARY KEY,
                        Name NVARCHAR(100) NOT NULL,
                        ManufacturerId INT NOT NULL FOREIGN KEY REFERENCES Manufacturers(Id)
                    );
                ";

                command.ExecuteNonQuery();
            }
        }
    }
}
