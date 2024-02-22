
namespace ConsoleAppHw2102ex1;

 class Program
{
    static ManualResetEvent generateNumbersEvent = new ManualResetEvent(false);
    static string inputFilePath = "numbers.txt";
    static string sumFilePath = "sums.txt";
    static string productFilePath = "products.txt";
    static void Main()
    {
        new Thread(GenerateNumbers).Start();
        Thread sumThread = new Thread(SumNumbers);
        Thread productThread = new Thread(ProductNumbers);
        sumThread.Start();
        productThread.Start();
        sumThread.Join();
        productThread.Join();
        Console.WriteLine("Генерация и расчеты завершены. Нажмите любую клавишу для выхода.");
        Console.ReadKey();
    }
    private static void GenerateNumbers(object? obj)
    {
        Console.WriteLine("Генерация чисел...");
        using (StreamWriter sw = new StreamWriter(inputFilePath))
        {
            Random rand = new Random();
            for (int i = 0; i < 10; i++) 
            {
                sw.WriteLine($"{rand.Next(1, 100)}, {rand.Next(1, 100)}");
            }
        }
        generateNumbersEvent.Set();// Сигнал о завершении генерации
    }
    private static void SumNumbers(object? obj)
    {
        generateNumbersEvent.WaitOne(); // Ожидание сигнала
        Console.WriteLine("Подсчет суммы чисел...");
        using (StreamReader sr = new StreamReader(inputFilePath))
        using (StreamWriter sw = new StreamWriter(sumFilePath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                var parts = line.Split(',');
                int sum = int.Parse(parts[0]) + int.Parse(parts[1].Trim());
                sw.WriteLine(sum);
            }
        }
    }
    private static void ProductNumbers(object? obj)
    {
        generateNumbersEvent.WaitOne();
        Console.WriteLine("Произведение пары чисел...");
        using (StreamReader sr = new StreamReader(inputFilePath))
        using (StreamWriter sw = new StreamWriter(productFilePath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                var parts = line.Split(',');
                int product = int.Parse(parts[0]) * int.Parse(parts[1].Trim());
                sw.WriteLine(product);
            }
        }
    }
}
