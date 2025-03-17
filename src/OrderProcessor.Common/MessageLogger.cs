using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Common
{
    public class MessageLogger
    {
        public void WriteMessage(string message)
        {
            Console.Write(message);
        }

        public void WriteMessageLine(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Success]: {message}");
            Console.ResetColor();
        }

        public void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[WARNING]: {message}");
            Console.ResetColor();
        }

        public void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR]: {message}");
            Console.ResetColor();
        }

    }
}
