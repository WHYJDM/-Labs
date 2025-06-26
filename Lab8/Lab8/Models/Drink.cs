namespace Lab8.Models
{
    /// <summary>
    /// Базовый класс для всех напитков.
    /// </summary>
    public abstract class Drink
    {
        /// <summary>Уникальный идентификатор напитка.</summary>
        public int Id { get; set; }

        /// <summary>Название напитка.</summary>
        public string Name { get; set; } = null!;

        /// <summary>Цена напитка.</summary>
        public decimal Price { get; set; }
    }
}
