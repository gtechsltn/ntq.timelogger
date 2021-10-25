using System;
using System.Diagnostics;

namespace Ntq.TimeLogger
{
    public static class StopwatchExtensions
    {
        /// <summary>
        /// Wrapping StopWatch timing with a delegate or lambda?
        /// https://stackoverflow.com/questions/232848/wrapping-stopwatch-timing-with-a-delegate-or-lambda
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="action"></param>
        /// <param name="iterations"></param>
        /// <returns></returns>
        public static long Time(this Stopwatch sw, Action action, int iterations)
        {
            sw.Reset();
            sw.Start();
            for (int i = 0; i < iterations; i++)
            {
                action();
            }
            sw.Stop();

            return sw.ElapsedMilliseconds;
        }

        /// <summary>
        /// Measuring method execution time
        /// https://codereview.stackexchange.com/questions/195823/measuring-method-execution-time
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stopwatch"></param>
        /// <param name="action"></param>
        /// <param name="elapsed"></param>
        /// <returns></returns>
        public static T Measure<T>(this Stopwatch stopwatch, Func<T> action, Action<TimeSpan> elapsed)
        {
            try
            {
                return action();
            }
            finally
            {
                elapsed(stopwatch.Elapsed);
            }
        }

        /// <summary>
        /// Measuring method execution time
        /// https://codereview.stackexchange.com/questions/195823/measuring-method-execution-time
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <param name="action"></param>
        /// <param name="elapsed"></param>
        public static void Measure(this Stopwatch stopwatch, Action action, Action<TimeSpan> elapsed)
        {
            stopwatch.Measure<object>(() => { action(); return default; }, elapsed);
        }
    }
}