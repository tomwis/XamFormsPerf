using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamFormsPerf.Pages.Layouts
{
    public partial class NestedStackLayoutTest : BasePage
    {
        int _controlsCount = 15;
        int _nestingLevels = 15;
        Random _random = new Random();

        public NestedStackLayoutTest(bool automated) : base(automated)
        {
            InitializeComponent();

            for(int i = 0; i < _controlsCount; ++i)
            {
                var r = _random.NextDouble();
                var g = _random.NextDouble();
                var b = _random.NextDouble();
                RootLayout.Children.Add(AddLayout(_nestingLevels, r, g, b));
            }
        }

        StackLayout AddLayout(int counter, double r, double g, double b)
        {
            var layout = new StackLayout();
            layout.BackgroundColor = new Color(r, g, b);
            layout.Margin = new Thickness(_nestingLevels - counter, 0, 0, 0);

            var label = new Label();
            label.Text = "Label in a nested StackLayout with text long enough to take more than 1 line: " + (_nestingLevels - counter); 
            layout.Children.Add(label);
            
            if (counter > 0)
            {
                r = _random.NextDouble();
                g = _random.NextDouble();
                b = _random.NextDouble();
                layout.Children.Add(AddLayout(--counter, r, g, b));
            }

            return layout;
        }
    }
}