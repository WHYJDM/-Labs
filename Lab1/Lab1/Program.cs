using System;
using System.IO;
using System.Reflection;

class Program
{
    const string DllFileName = "Lab1.dll";
    const string ManufacturerClassName = "Lab1.Manufacturer";
    const string DrinkClassName = "Lab1.Drink";
    const string CreateMethodName = "Create";
    const string PrintMethodName = "PrintObject";

    const string ManufacturerName = "Coca-Cola";
    const string ManufacturerAddress = "Atlanta";
    const bool IsAChildCompany = false;

    const int DrinkId = 1;
    const string DrinkName = "Sprite";
    const string SerialNumber = "SN123456";
    const string DrinkType = "Soft";

    static void Main()
    {
        try
        {
            string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DllFileName);
            if (!File.Exists(dllPath))
            {
                Console.WriteLine($"File {DllFileName} not found at path: {dllPath}");
                return;
            }

            Assembly assembly = Assembly.LoadFrom(dllPath);

            Console.WriteLine("Classes in the assembly:");
            foreach (Type type in assembly.GetTypes())
            {
                Console.WriteLine(type.FullName);
                Console.WriteLine("Properties:");
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    Console.WriteLine($" - {prop.Name} ({prop.PropertyType.Name})");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Task 3: Creating and printing objects");

            Type manufacturerType = assembly.GetType(ManufacturerClassName);
            MethodInfo createManufacturer = manufacturerType.GetMethod(CreateMethodName);
            object manufacturer = createManufacturer.Invoke(null, new object[]
            {
                ManufacturerName,
                ManufacturerAddress,
                IsAChildCompany
            });

            Type drinkType = assembly.GetType(DrinkClassName);
            MethodInfo createDrink = drinkType.GetMethod(CreateMethodName);
            object drink = createDrink.Invoke(null, new object[]
            {
                DrinkId,
                DrinkName,
                SerialNumber,
                DrinkType,
                manufacturer
            });

            MethodInfo printMethod = drinkType.GetMethod(PrintMethodName);
            printMethod.Invoke(drink, null);

            RunMethodInvoker(assembly);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void RunMethodInvoker(Assembly asm)
    {
        Console.WriteLine("\nTask 1: Invoke methods via input");

        while (true)
        {
            Console.Write("Enter full class name (e.g., Lab1.Drink): ");
            string className = Console.ReadLine();

            Console.Write("Enter method name (e.g., Create): ");
            string methodName = Console.ReadLine();

            Console.Write("Enter arguments separated by commas (e.g., 1,Sprite,SN123456,Soft): ");
            string input = Console.ReadLine();
            string[] args = string.IsNullOrWhiteSpace(input) ? Array.Empty<string>() : input.Split(',');

            Type type = asm.GetType(className);
            if (type == null)
            {
                ShowRetryMessage();
                continue;
            }

            MethodInfo method = type.GetMethod(methodName);
            if (method == null)
            {
                ShowRetryMessage();
                continue;
            }

            ParameterInfo[] parameters = method.GetParameters();
            object[] parsedArgs = new object[parameters.Length];

            try
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (i < args.Length && !string.IsNullOrWhiteSpace(args[i]))
                    {
                        parsedArgs[i] = Convert.ChangeType(args[i], parameters[i].ParameterType);
                    }
                }
            }
            catch
            {
                ShowRetryMessage();
                continue;
            }

            try
            {
                object instance = method.IsStatic ? null : Activator.CreateInstance(type);
                object result = method.Invoke(instance, parsedArgs);

                Console.WriteLine("Method executed successfully.");
                if (result != null)
                    Console.WriteLine("Result: " + result);

                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                ShowRetryMessage();
            }
        }
    }


    static void ShowRetryMessage()
    {
        Console.WriteLine("You entered an incorrect class name, method name, or arguments. Please try again.\n");
    }
}
