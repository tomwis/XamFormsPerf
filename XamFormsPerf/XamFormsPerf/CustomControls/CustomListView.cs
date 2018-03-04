using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamFormsPerf.CustomControls
{
    public class CustomListView : ListView, ICustomListView
    {
        Func<Task> ICustomListView.ScrollToEnd { get; set; }

        public async Task ScrollListToEnd()
        {
            var asInterface = this as ICustomListView;
            if (asInterface.ScrollToEnd != null)
            {
                await asInterface.ScrollToEnd.Invoke();
            }
        }
    }
}
