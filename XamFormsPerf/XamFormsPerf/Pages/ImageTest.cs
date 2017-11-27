using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamFormsPerf.Pages
{
    class ImageTest : ControlTestBase<Image>
    {
        public ImageTest(bool automated) : base(automated)
        {
        }

        protected override Image CreateView()
        {
            return new Image
            {
                Source = ImageSource.FromFile("ic_schedule_black_48dp.png")
            };
        }
    }
}
