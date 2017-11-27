using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamFormsPerf.Pages
{
    public partial class ControlTestBase<TControl> : ContentPage where TControl : View
    {
        const int _childCount = 200;
        readonly bool _automated;

        public ControlTestBase(bool automated)
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

            _automated = automated;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            PerfLog.MeasureStop(GetType().Name);

            if (_automated)
            {
                await Navigation.PopAsync();
            }
        }

        protected virtual TControl CreateView()
        {
            return default(TControl);
        }
    }
}
