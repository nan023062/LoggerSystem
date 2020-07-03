using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSharpLogSystem
{
    public class NativeLogConsole : IDebugerConsole
    {
        public NativeLogConsole(){ }

        public void Log(string msg)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);
            Console.ForegroundColor = old;
        }

        public void LogWarning(string msg)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
            Console.ForegroundColor = old;
        }

        public void LogError(string msg)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ForegroundColor = old;
        }
    }
}
