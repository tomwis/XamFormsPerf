using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XamForms.Perf.Web.Models
{
    public class TestTablePoint
    {
        public string Version { get; set; }
        public int AvgMs { get; set; }
        public double DiifFromLast { get; set; }
    }
}