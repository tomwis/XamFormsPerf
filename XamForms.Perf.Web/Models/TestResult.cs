using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XamForms.Perf.Web.Models
{
    public class TestResult
    {
        public string Name { get; set; }
        public int AvgMs { get; set; }
        public DateTime Date { get; set; }
        public Version Version { get; set; }
        public string Target { get; set; }
        public string Model { get; set; }
        public string OsVersion { get; set; }
    }
}