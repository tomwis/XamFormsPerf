using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using System.Diagnostics;
using System.Collections.Generic;

namespace XamFormsPerf.UITests
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)] // not supported on Windows, will have to do it later
    public class Tests
    {
        IApp _app;
        Platform _platform;
        const int _loops = 3;
        int _currentLoop = 1;

        public Tests(Platform platform)
        {
            _platform = platform;

            var testTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-ffff");
            _resultsFilename = string.Format(RESULTS_FILENAME_TEMPLATE, testTime);
            _resultsAvgFilename = string.Format(RESULTS_AVERAGE_FILENAME_TEMPLATE, testTime);
        }

        [SetUp]
        public void BeforeEachTest()
        {
            if(_app != null)
            {
                if (_platform == Platform.Android)
                    _app.Invoke("ExitApp");
                else
                    _app.Invoke("exitApp:");
            }
            _app = AppInitializer.StartApp(_platform);
        }

        [Test]
        public void AppLaunches()
        {
            _app.WaitForElement("FinishedTestsId");

            var summary = SaveResults();

            if (_currentLoop < _loops)
            {
                ++_currentLoop;
                BeforeEachTest();
                AppLaunches();
            }
            else
            {
                Debug.WriteLine(summary);
                ComputeAverageResultsAndSave();
            }
        }

        string SaveResults()
        {
            var summary = _app.Invoke("TestsSummary").ToString();
            var lines = summary.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            List<string> entries = new List<string>();
            entries.Add(RESULTS_HEADER);

            foreach (var line in lines)
            {
                var parts = line.Split("():".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                entries.Add(string.Join(";", new[] { parts[0].Trim(), parts[2].Trim().Replace(" ms", "") }));
            }

            File.AppendAllLines(_resultsFilename, entries);

            return summary;
        }

        void ComputeAverageResultsAndSave()
        {
            var results = File.ReadAllLines(_resultsFilename);
            Dictionary<string, List<int>> tests = new Dictionary<string, List<int>>(results.Where(s => !s.StartsWith(RESULTS_HEADER)).Select(s =>
            {
                var parts = s.Split(";".ToCharArray(), StringSplitOptions.None);
                return new
                {
                    Name = parts[0],
                    AvgMs = int.Parse(parts[1])
                };
            }).GroupBy(t => t.Name, g => g.AvgMs).ToDictionary(k => k.Key, v => v.ToList()));
            var avgResults = tests.Select(s => new { Name = s.Key, AvgMs = s.Value.Sum() / s.Value.Count });
            File.AppendAllLines(_resultsAvgFilename, avgResults.Select(s => $"{s.Name};{s.AvgMs}"));

        }
        
        readonly string _resultsFilename;
        readonly string _resultsAvgFilename;
        const string RESULTS_HEADER = "Test name;Avg ms";
        const string RESULTS_FILENAME_TEMPLATE = "results_{0}.csv";
        const string RESULTS_AVERAGE_FILENAME_TEMPLATE = "resultsAvg_{0}.csv";
    }
}

