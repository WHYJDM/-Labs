namespace Lab8.Models
{
    /// <summary>
    /// Алкогольный напиток: содержит информацию о крепости.
    /// </summary>
    public class AlcoholicDrink : Drink
    {
        /// <summary>Крепость напитка в процентах.</summary>
        public double AlcoholPercentage { get; set; }
    }
}
