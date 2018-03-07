using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamFormsPerf.Pages.ListView
{
    public class ListViewBasePage : ContentPage
    {
        readonly bool _automated;

        protected Func<Task> ScrollToEnd { get; set; }

        public ListViewBasePage(bool automated)
        {
            _automated = automated;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Task.Delay(100);

            await ScrollToEnd?.Invoke();
            EndTest();
        }

        protected async void EndTest()
        {
            PerfLog.MeasureStop(GetType().Name);

            if (_automated)
            {
                await Navigation.PopAsync();
            }
        }
    }
}
