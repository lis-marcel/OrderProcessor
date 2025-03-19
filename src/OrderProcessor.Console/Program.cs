using OrderProcessor.Common;
using OrderProcessor.Service;

namespace OrderProcessor
{
    internal class Program
    {
        private static readonly ConsoleLogger messageLogger = new();

        static void Main(string[] args)
        {
            try
            {
                ConsoleService service = new();
                service.Start();
            }
            catch (Exception ex)
            {
                messageLogger.WriteError(ex.Message);
            }
        }
    }
}
