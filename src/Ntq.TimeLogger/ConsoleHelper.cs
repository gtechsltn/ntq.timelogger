using System;

namespace Ntq.TimeLogger
{
    /// <summary>
    /// Measuring method execution time
    /// https://codereview.stackexchange.com/questions/195823/measuring-method-execution-time
    /// </summary>
    public static class ConsoleHelper
    {
        public static void WriteElapsed(TimeSpan elapsed) => Console.WriteLine(elapsed);

        public static Action<TimeSpan> WriteElapsed(string memberName)
        {
            return elapsed => Console.WriteLine($"'{memberName}' executed in {elapsed.TotalMilliseconds} milliseconds");
        }
    }
}