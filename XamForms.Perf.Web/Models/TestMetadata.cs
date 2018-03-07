using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XamForms.Perf.Web.Models
{
    public class TestMetadata
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public TestMetadata(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}