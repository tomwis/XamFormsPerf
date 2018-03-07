using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamFormsPerf.Pages.ListView
{
    public partial class ListViewSwitchCellTest : ListViewBasePage
    {
        const int _elementsCount = 200;

        public ListViewSwitchCellTest(bool automated) : base(automated)
        {
            InitializeComponent();
            
            List1.ItemsSource = Enumerable.Range(0, _elementsCount).Select(s => new { Text = $"Text {s}", On = s % 2 == 0 });

            ScrollToEnd = async () => await List1.ScrollListToEnd();
        }
    }
}
