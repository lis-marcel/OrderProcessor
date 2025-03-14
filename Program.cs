using OrderProcessor.Service;

namespace OrderProcessor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConsoleService service = new();
                service.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
