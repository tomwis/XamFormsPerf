using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamFormsPerf.Pages.ListView
{
    public partial class ListViewExtendedViewCellTest : ListViewBasePage
    {
        const int _elementsCount = 200;
        readonly Color[] colors = new Color[] { Color.LightGreen, Color.LightCoral, Color.LightBlue, Color.LightCyan, Color.LightGoldenrodYellow, Color.LightPink, Color.LightSteelBlue };

        public ListViewExtendedViewCellTest(bool automated) : base(automated)
        {
            InitializeComponent();

            List1.ItemsSource = Enumerable.Range(0, _elementsCount).Select(s => new
            {
                Background = Color.FromHex("#eeeeee"),
                StatusColor = colors[s % colors.Length],
                Title = $"Title {s}",
                Description = $"Long Description Long Description Long Description {s}",
                Date = DateTime.Now,
                IconTitle = "ic_schedule_black_48dp.png",
                Icon1 = "ic_schedule_black_48dp.png",
                Icon2 = "ic_schedule_black_48dp.png",
                Icon3 = "ic_schedule_black_48dp.png",
                Icon4 = "ic_schedule_black_48dp.png",
                StatusIcon = "ic_schedule_black_48dp.png",

                Status1Color = Color.LightGreen,
                Status1Icon = "ic_schedule_black_48dp.png",
                Status1Label = "Status 1",

                Status2Color = Color.LightGray,
                Status2Icon = "ic_schedule_black_48dp.png",
                Status2Label = "Status 2",

                Status3Color = Color.LightPink,
                Status3Icon = "ic_schedule_black_48dp.png",
                Status3Label = "Status 3",

                Status4Color = Color.LightSkyBlue,
                Status4Icon = "ic_schedule_black_48dp.png",
                Status4Label = "Status 4",
            });

            ScrollToEnd = async () => await List1.ScrollListToEnd();
        }
    }
}
