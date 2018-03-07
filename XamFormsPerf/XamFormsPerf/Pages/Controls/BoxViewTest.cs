using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamFormsPerf.Pages.Controls
{
    class BoxViewTest : ControlTestBase<BoxView>
    {
        public BoxViewTest(bool automated) : base(automated)
        {
        }

        protected override BoxView CreateView()
        {
            return new BoxView
            {
                BackgroundColor = Color.Purple
            };
        }
    }
}
