using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Ntq.TimeLogger.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TimeLoggerTest();
            StopwatchTest01();
            StopwatchTest02();
            StopwatchTest03();
            UsingStopWatchUsage();
        }

        private static void StopwatchTest03()
        {
            long time = TimerDelegateCommand.RunAction(DoStuff);
            Console.WriteLine("Time: " + time);

            time = TimerDelegateCommand.RunAction(delegate
            {
                // write your code here
            });
            Console.WriteLine("Time: " + time);

            Console.WriteLine("\r\nPress any key to exit ...");
            Console.ReadLine();
        }

        /// <summary>
        /// Great Uses of Using Statement in C#
        /// https://ardalis.com/great-uses-of-using-statement-in-c/
        /// </summary>
        public static void BasicStopWatchUsage()
        {
            Console.WriteLine("Basic StopWatch Used: ");
            var stopWatch = Stopwatch.StartNew();
            Thread.Sleep(3000);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                                ts.Hours, ts.Minutes, ts.Seconds,
                                                ts.Milliseconds / 10);
            Console.WriteLine(elapsedTime, "RunTime");
        }

        /// <summary>
        /// Great Uses of Using Statement in C#
        /// https://ardalis.com/great-uses-of-using-statement-in-c/
        /// </summary>
        private static void UsingStopWatchUsage()
        {
            Console.WriteLine("ConsoleAutoStopWatch Used: ");
            using (new ConsoleAutoStopWatch())
            {
                Thread.Sleep(3000);
            }
        }

        private static void TimeLoggerTest()
        {
            if (!Directory.Exists(TimeLogger.LogFolderPath))
            {
                Directory.CreateDirectory(TimeLogger.LogFolderPath);
            }
            TimeLogger.Init("Main");
            TimeLogger.Start($"{nameof(Program.TimeLoggerDoStuff)}");
            TimeLoggerDoStuff();
            TimeLogger.Stop($"{nameof(Program.TimeLoggerDoStuff)}");
            TimeLogger.Summary();
        }

        private static void StopwatchTest01()
        {
            Stopwatch
                .StartNew()
                .Measure(DoStuff, ConsoleHelper.WriteElapsed);
        }

        private static void StopwatchTest02()
        {
            Stopwatch
                .StartNew()
                .Measure(DoStuff, ConsoleHelper.WriteElapsed(nameof(DoStuff)));
        }

        private static void TimeLoggerDoStuff()
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