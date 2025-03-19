using OrderProcessor.Common;
using OrderProcessor.Service;

namespace OrderProcessor.Console
{
    internal class Program
    {
        private static readonly ConsoleLogger consoleLogger = new();

        static void Main(string[] args)
        {
            try
            {
                ConsoleService service = new();
                service.Start();
            }
            catch (Exception ex)
            {
                consoleLogger.WriteError(ex.Message);
            }
        }
    }
}
