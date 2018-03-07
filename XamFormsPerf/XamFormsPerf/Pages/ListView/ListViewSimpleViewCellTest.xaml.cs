using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamFormsPerf.Pages.ListView
{
    public partial class ListViewSimpleViewCellTest : ListViewBasePage
    {
        const int _elementsCount = 200;

        public ListViewSimpleViewCellTest(bool automated) : base(automated)
        {
            InitializeComponent();
            
            List1.ItemsSource = Enumerable.Range(0, _elementsCount).Select(s => new { Title = $"Title {s}", Description = $"Description {s}", Date = DateTime.Now, Icon = "ic_schedule_black_48dp.png" });

            ScrollToEnd = async () => await List1.ScrollListToEnd();
        }
    }
}
