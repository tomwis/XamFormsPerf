using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamFormsPerf.Pages.ListView
{
    public partial class ListViewEntryCellTest : ListViewBasePage
    {
        const int _elementsCount = 200;

        public ListViewEntryCellTest(bool automated) : base(automated)
        {
            InitializeComponent();
            
            List1.ItemsSource = Enumerable.Range(0, _elementsCount).Select(s => new { Label = $"Label {s}", Placeholder = $"Placeholder {s}", Text = s % 2 == 0 ? $"Text {s}" : null, Keyboard = Keyboard.Numeric });

            ScrollToEnd = async () => await List1.ScrollListToEnd();
        }
    }
}
