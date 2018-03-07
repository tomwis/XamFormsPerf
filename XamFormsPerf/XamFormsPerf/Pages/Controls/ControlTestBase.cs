using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamFormsPerf.Pages.Controls
{
    public partial class ControlTestBase<TControl> : BasePage where TControl : View
    {
        const int _childCount = 200;

        public ControlTestBase(bool automated) : base(automated)
        {
            var panel = new StackLayout();
            Content = new ScrollView
            {
                Content = panel
            };

            for (int i = 0; i < _childCount; ++i)
            {
                panel.Children.Add(CreateView());
            }
        }

        protected virtual TControl CreateView()
        {
            return default(TControl);
        }
    }
}
