using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

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

            _testsDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            var testTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-ffff");
            _resultsFilename = string.Format(RESULTS_FILENAME_TEMPLATE, testTime);
            _resultsAvgFilename = string.Format(RESULTS_AVERAGE_FILENAME_TEMPLATE, testTime);
            (_formsVersion, _targetFramework) = GetFormsVersion();
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

            foreach (var line in lines)
            {
                var parts = line.Split("():".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                entries.Add(string.Join(";", new[] 
                {
                    parts[0].Trim(), // test name
                    parts[2].Trim().Replace(" ms", ""), // time in ms
                    _testsDate,
                    _formsVersion,
                    _targetFramework
                }));
            }

            if(!File.Exists(_resultsFilename))
            {
                entries.Insert(0, RESULTS_HEADER);
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

            var (_formsVersion, _targetFramework) = GetFormsVersion();

            File.AppendAllLines(_resultsAvgFilename, avgResults.Select(s => $"{s.Name};{s.AvgMs};{_testsDate};{_formsVersion};{_targetFramework}"));
        }

        (string, string) GetFormsVersion()
        {
            var packagesConfigPath = $@"..\..\..\XamFormsPerf\XamFormsPerf.{_platform}\packages.config";
            var xml = XDocument.Load(packagesConfigPath);
            var formsPackage = xml.Root.Elements("package").FirstOrDefault(s => s.Attribute("id").Value == "Xamarin.Forms");
            var formsVersion = formsPackage.Attribute("version").Value;
            var targetFramework = formsPackage.Attribute("targetFramework").Value;
            return (formsVersion, targetFramework);
        }

        readonly string _formsVersion, _targetFramework;
        readonly string _resultsFilename, _resultsAvgFilename;
        readonly string _testsDate;
        const string RESULTS_HEADER = "Test name;Avg ms;Date;FormsVersion;TargetFramework";
        const string RESULTS_FILENAME_TEMPLATE = "results_{0}.csv";
        const string RESULTS_AVERAGE_FILENAME_TEMPLATE = "resultsAvg_{0}.csv";
    }
}

