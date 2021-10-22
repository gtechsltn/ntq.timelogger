using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Ntq.TimeLogger
{
    public static class PerfConfig
    {
        /// <summary>
        /// DELETE ME IN PRODUCTION
        /// USE_V1 = true  : Before optimize code
        /// USE_V1 = false : After optimize code
        /// </summary>
        public static bool USE_V1 = true;
    }

    public class TimeLogger
    {
        private const string DELIMITER = "\t";
        private string methodName;
        private static string functionName;
        private TimeLogger parent;
        private Stopwatch stw;

        private static TimeLogger cur;
        private static TimeLogger root;

        private Dictionary<string, TimeLogger> children;

        private static StringBuilder summaryLog = new StringBuilder();

        public const string LogFolderPath = @"C:\log\perf-log";

        public TimeLogger(string name, TimeLogger parent)
        {
            this.methodName = name;
            stw = new Stopwatch();
            this.parent = parent;
        }

        private void StartMethod(string name)
        {
            if (name.Equals(this.methodName)) // is current -> recursive method
            {
                stw.Start();
                cur = this;
            }
            else // find child
            {
                if (stw.IsRunning)
                {
                    if (children == null)
                    {
                        children = new Dictionary<string, TimeLogger>();
                    }
                    TimeLogger child;
                    if (!children.TryGetValue(name, out child))
                    {
                        child = new TimeLogger(name, this);
                        children[name] = child;
                    }
                    child.Start();
                    cur = child;
                }
                else
                {
                    if (parent != null) // for brothers, backup only, not go into this case if caller correctly
                    {
                        parent.StartMethod(name);
                    }
                    else
                    {
                        Debug.WriteLine("[NTQPT] Wrong call");
                        summaryLog.AppendLine("[NTQPT] Wrong call");
                    }
                }
            }
        }

        private void Start()
        {
            stw.Start();
        }

        private void StopMethod()
        {
            if (stw.IsRunning)
            {
                stw.Stop();
            }
            else
            {
                if (parent != null)
                {
                    cur = parent;
                    cur.StopMethod();
                }
            }
        }

        private void SummaryMethod(string tab = DELIMITER)
        {
            Debug.WriteLine("{0}{1}{2}", stw.ElapsedMilliseconds, tab, methodName);
            summaryLog.AppendLine().AppendFormat("{0}{1}{2}", stw.ElapsedMilliseconds, tab, methodName);
            if (children != null)
            {
                foreach (var item in children)
                {
                    item.Value.SummaryMethod(tab + DELIMITER);
                }
            }
        }

        public static void Init(string apiName)
        {
            functionName = apiName;
            summaryLog.Clear();
            root = new TimeLogger(apiName, null); // root
            cur = root;
            cur.StartMethod(apiName);
        }

        public static void Start(string name)
        {
            try
            {
                if (root != null && cur != null)
                {
                    cur.StartMethod(name);
                }
            }
            catch
            {
            }
        }

        public static void Stop(string method = null)
        {
            try
            {
                if (root != null && cur != null)
                {
                    cur.StopMethod();
                }
            }
            catch
            {
            }
        }

        public static void Summary()
        {
            root.StopMethod();
            summaryLog.AppendLine("[NTQPT] API execution times summary");
            Debug.WriteLine("[NTQPT] API execution times summary");
            root.SummaryMethod();
            root = null;
            WriteFileLog(summaryLog.ToString());
        }

        private static void WriteFileLog(string msg)
        {
            string filename = $"perf-log.{functionName}-{DateTime.Now.ToString("MMdd-hhmmss")}.txt";
            File.WriteAllText(Path.Combine(TimeLogger.LogFolderPath, filename), msg, Encoding.UTF8);
        }
    }
}
