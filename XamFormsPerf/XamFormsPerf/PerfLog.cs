using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace XamFormsPerf
{
    public static class PerfLog
    {
        const string LogFile = "log.txt";
        static string Content { get; set; }
        static Dictionary<string, Stopwatch> _measurements = new Dictionary<string, Stopwatch>();
        static Dictionary<string, List<long>> _results = new Dictionary<string, List<long>>();

        public static void Measure(string key, Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Restart();
            action();
            stopwatch.Stop();
            Add(key, "", stopwatch.ElapsedMilliseconds);
        }

        public static void MeasureStart(string key)
        {
            MeasureStopIfExists(key);

            _measurements[key] = new Stopwatch();
            _measurements[key].Start();
        }

        public static void MeasurePoint(string key, [CallerMemberName] string tag = "")
        {
            if (_measurements.ContainsKey(key) && _measurements[key] != null && _measurements[key].IsRunning)
            {
                Add(key, tag, _measurements[key].ElapsedMilliseconds);
            }
        }

        public static void MeasureStop(string key)
        {
            if (MeasureStopIfExists(key))
            {
                Add(key, "", _measurements[key].ElapsedMilliseconds);
            }
        }

        static bool MeasureStopIfExists(string key)
        {
            if (_measurements.ContainsKey(key) && _measurements[key] != null && _measurements[key].IsRunning)
            {
                _measurements[key].Stop();
                return true;
            }
            return false;
        }

        static void Add(string key, string tag, long value)
        {
            if (Content == null)
                Content = "";

            var newEntry = key.ToString();
            if (!string.IsNullOrEmpty(tag))
            {
                newEntry += $" - {tag}";
            }
            newEntry += $": {value}";
            Content += newEntry;
            Content += System.Environment.NewLine;

            if (!string.IsNullOrEmpty(tag))
            {
                tag = "_" + tag;
            }

            var resultKey = $"{key}{tag}";
            if (!_results.ContainsKey(resultKey))
            {
                _results[resultKey] = new List<long>();
            }
            _results[resultKey].Add(value);

            Debug.WriteLine("[PerfLog] " + newEntry);
        }

        public static string Summary()
        {
            return string.Join(Environment.NewLine, _results.Select(s => s.Key + $" ({s.Value.Count}): " + s.Value.Average() + " ms"));
        }
    }
}
