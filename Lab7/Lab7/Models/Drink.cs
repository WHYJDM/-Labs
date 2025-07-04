﻿namespace Lab7.Models
{
    public class Drink
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int ManufacturerId { get; set; }

        public Manufacturer Manufacturer { get; set; } = null!;
    }
}
