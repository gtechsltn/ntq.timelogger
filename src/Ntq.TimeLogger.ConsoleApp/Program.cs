using System;
using System.Diagnostics;
using System.IO;

namespace Ntq.TimeLogger.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (!Directory.Exists(TimeLogger.LogFolderPath))
            {
                Directory.CreateDirectory(TimeLogger.LogFolderPath);
            }
            TimeLogger.Init("Main");
            TimeLogger.Start($"{nameof(Program.StopwatchTest)}");
            StopwatchTest();
            TimeLogger.Stop($"{nameof(Program.StopwatchTest)}");
            TimeLogger.Summary();
        }

        private static void StopwatchTest()
        {
            var s = new Stopwatch();
            var miliSeconds = s.Time(() => DoStuff(), 100);
            Console.WriteLine($"It takes {miliSeconds} miliseconds");
        }

        private static void DoStuff()
        {
            TimeLogger.Start($"{nameof(Program.DoStuff)}");
            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine($"{i}");
            }
            TimeLogger.Stop($"{nameof(Program.DoStuff)}");
        }
    }
}