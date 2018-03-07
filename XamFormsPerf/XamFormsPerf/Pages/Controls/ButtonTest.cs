using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamFormsPerf.Pages.Controls
{
    class ButtonTest : ControlTestBase<Button>
    {
        public ButtonTest(bool automated) : base(automated)
        {
        }

        protected override Button CreateView()
        {
            var button = new Button
            {
                Text = "Click me!"
            };
            button.Clicked += Button_Clicked;
            return button;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Yay!", "You clicked me!", "Done");
        }
    }
}
