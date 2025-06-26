using System;

namespace Lab8
{
    /// <summary>
    /// Константы для наименований таблиц и дискриминатора.
    /// </summary>
    internal static class MappingConstants
    {
        public const string TableDrinks = "Drinks";
        public const string TableAlcoholicDrinks = "AlcoholicDrinks";
        public const string TableNonAlcoholicDrinks = "NonAlcoholicDrinks";
        public const string DiscriminatorColumn = "DrinkType";
        public const string DiscriminatorValueAlcoholic = "Alcoholic";
        public const string DiscriminatorValueNonAlcoholic = "NonAlcoholic";
    }


/// <summary>
/// Точка входа. 
/// Заглушка для работы EF CLI и миграций.
/// </summary>
internal static class Program
    {
        private const string StartupMessage =
            "Используйте EF CLI (dotnet ef) для управления миграциями.";

        /// <summary>Точка входа приложения.</summary>
        public static void Main(string[] args)
        {
            Console.WriteLine(StartupMessage);
        }
    }
}
