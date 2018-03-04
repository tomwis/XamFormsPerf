using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamFormsPerf.Pages.Controls
{
    class LabelTest : ControlTestBase<Label>
    {
        public LabelTest(bool automated) : base(automated)
        {
        }

        protected override Label CreateView()
        {
            return new Label
            {
                Text = "1"
            };
        }
    }
}
