using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XamForms.Perf.Web.Models;

namespace XamForms.Perf.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var results = ReadResultsFromFiles();
            var tests = GroupResultsIntoTests(results);
            return View(tests);
        }

        List<TestResult> ReadResultsFromFiles()
        {
            var data = Server.MapPath("~/App_Data/");
            var files = Directory.GetFiles(data, "resultsAvg*.csv");

            List<TestResult> results = new List<TestResult>();
            foreach (var file in files)
            {
                var lines = System.IO.File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    var parts = line.Split(";".ToCharArray());

                    results.Add(new TestResult
                    {
                        Name = parts[0],
                        AvgMs = int.Parse(parts[1]),
                        Date = DateTime.Parse(parts[2], CultureInfo.InvariantCulture),
                        Version = Version.Parse(parts[3]),
                        Target = parts[4]
                    });
                }
            }

            return results;
        }

        Dictionary<string, List<TestTablePoint>> GroupResultsIntoTests(List<TestResult> results)
        {
            Dictionary<string, List<TestTablePoint>> tests = new Dictionary<string, List<TestTablePoint>>();

            var groups = results.GroupBy(s => s.Name);

            foreach (var group in groups)
            {
                var myGroup = group.OrderByDescending(s => s.Version).ToList();
                var items = new List<TestTablePoint>();
                for (int i = 0; i < myGroup.Count; ++i)
                {
                    double diffPercent = Double.MaxValue;
                    if (i < myGroup.Count - 1)
                    {
                        var diff = myGroup[i + 1].AvgMs - myGroup[i].AvgMs;
                        diffPercent = Math.Round((double)diff / myGroup[i + 1].AvgMs, 2);
                    }

                    items.Add(new TestTablePoint
                    {
                        Version = myGroup[i].Version.ToString(),
                        AvgMs = myGroup[i].AvgMs,
                        DiifFromLast = diffPercent
                    });
                }

                tests.Add(group.Key, items);
            }

            return tests;
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}