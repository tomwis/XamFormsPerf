﻿using System;
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
        public ActionResult Index(string platform)
        {
            var results = ReadResultsFromFiles(platform);
            var tests = GroupResultsIntoTests(results);
            return View(tests);
        }

        IEnumerable<TestResult> ReadResultsFromFiles(string targetFramework)
        {
            if (targetFramework == null) return Enumerable.Empty<TestResult>();

            var data = Server.MapPath("~/App_Data/");
            var files = Directory.GetFiles(data, "resultsAvg*.csv");
            var descriptions = GetDescriptions(data);

            List<TestResult> results = new List<TestResult>();
            foreach (var file in files)
            {
                var lines = System.IO.File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    var parts = line.Split(";".ToCharArray());
                    var metadata = descriptions.FirstOrDefault(s => s.Id == parts[0]);                    

                    if (parts[5].ToLower() == targetFramework.ToLower())
                    {
                        results.Add(new TestResult
                        {
                            Id = metadata.Id,
                            Name = metadata.Name,
                            Description = metadata.Description,
                            AvgMs = int.Parse(parts[1]),
                            Date = DateTime.Parse(parts[2], CultureInfo.InvariantCulture),
                            Version = Version.Parse(parts[3]),
                            Target = parts[4],
                            Model = parts[6],
                            OsVersion = parts[7]
                        });
                    }
                }
            }

            return results;
        }

        IEnumerable<TestMetadata> GetDescriptions(string dataPath)
        {
            var descriptions = System.IO.File.ReadLines(Path.Combine(dataPath, "descriptions.csv"));

            return descriptions.Select(s =>
            {
                var parts = s.Split(';');

                return new TestMetadata(parts[0], parts[1], parts[2]);
            });
        }

        Dictionary<TestMetadata, List<TestTablePoint>> GroupResultsIntoTests(IEnumerable<TestResult> results)
        {
            var tests = new Dictionary<TestMetadata, List<TestTablePoint>>();

            var groups = results.GroupBy(s => s.Id);

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
                        DiifFromLast = diffPercent,
                        Model = myGroup[i].Model,
                        OsVersion = myGroup[i].OsVersion,
                        Description = myGroup[i].Description
                    });
                }

                var testResultGroup = myGroup.First();
                tests.Add(new TestMetadata(testResultGroup.Id, testResultGroup.Name, testResultGroup.Description), items);
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