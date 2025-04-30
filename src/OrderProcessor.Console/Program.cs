using OrderProcessor.Service;

namespace OrderProcessor.Console
{
    internal class Program
    {

        static void Main(string[] args)
        {
            //AppDomain currentDomain = AppDomain.CurrentDomain;
            //currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

            //try
            //{
            //    ConsoleService service = new();
            //    service.Start();
            //}
            //catch (Exception ex)
            //{
            //    consoleLogger.WriteError(ex.Message);
            //}
        }

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            System.Console.WriteLine("Exception handler caught : " + e.Message);
            System.Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
        }
    }
}
