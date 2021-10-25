using System;
using System.Diagnostics;

namespace Ntq.TimeLogger
{
    /// <summary>
    /// Class for executing code and measuring the time it takes to run
    /// https://gunnarpeipman.com/find-out-for-how-long-your-method-runs/
    /// </summary>
    public class TimerDelegateCommand
    {
        private readonly Stopwatch _stopper = new Stopwatch();

        /// <summary>
        /// Runs given actions and measures time. Inherited classes
        /// may override this method.
        /// </summary>
        /// <param name="action">Action to run.</param>
        public virtual void Run(Action action)
        {
            _stopper.Reset();
            _stopper.Start();

            try
            {
                action.Invoke();
            }
            finally
            {
                _stopper.Stop();
            }
        }

        /// <summary>
        /// Static version of action runner. Can be used for "one-line"
        /// measurings.
        /// </summary>
        /// <param name="action">Action to run.</param>
        /// <returns>Returns time that action took to run in
        /// milliseconds.</returns>
        public static long RunAction(Action action)
        {
            var instance = new TimerDelegateCommand();
            instance.Run(action);
            return instance.Time;
        }

        /// <summary>
        /// Gets the action running time in milliseconds.
        /// </summary>
        public long Time
        {
            get { return _stopper.ElapsedMilliseconds; }
        }

        /// <summary>
        /// Gets the stopwatch instance used by this class.
        /// </summary>
        public Stopwatch Stopper
        {
            get { return _stopper; }
        }
    }
}