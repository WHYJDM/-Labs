namespace Lab8.Models
{
    /// <summary>
    /// Безалкогольный напиток: содержит информацию о содержании сахара.
    /// </summary>
    public class NonAlcoholicDrink : Drink
    {
        /// <summary>Содержание сахара (грамм на 100 мл).</summary>
        public double SugarPer100ml { get; set; }
    }
}
