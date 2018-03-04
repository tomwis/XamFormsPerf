using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamFormsPerf.Pages
{
    public class BasePage : ContentPage
    {
        readonly bool _automated;

        public BasePage(bool automated)
        {
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
    }
}
