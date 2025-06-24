using System.Collections.Generic;

namespace Lab7.Models
{
    public class Manufacturer
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public ICollection<Drink> Drinks { get; set; } = new List<Drink>();
    }
}
