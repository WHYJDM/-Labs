namespace Lab1
{
    public class Drink
    {
        private int ID = 0;
        public string Name { get; set; } = "Zibert";
        public string SerialNumber { get; set; } = "zxc123";
        public string DrinkType { get; set; } = "nesoda";

        public Manufacturer? Manufacturer { get; set; } = null;

        public static Drink Create(int id, string name, string serialNumber, string drinkType, Manufacturer manufacturer)
        {
            return new Drink { ID = id, Name = name, SerialNumber = serialNumber, DrinkType = drinkType, Manufacturer = manufacturer };
        }

        public void PrintObject()
        {
            Console.WriteLine($"Drink: Name={Name}, SerialNumber={SerialNumber}, DrinkType={DrinkType}, Manufacturer={Manufacturer?.Name ?? "None"}");
        }
    }
}
