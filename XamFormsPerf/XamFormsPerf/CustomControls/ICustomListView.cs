using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamFormsPerf.CustomControls
{
    public interface ICustomListView
    {
        Func<Task> ScrollToEnd { get; set; }
    }
}
