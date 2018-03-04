using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using XamFormsPerf.CustomControls;
using XamFormsPerf.Droid.CustomRenderers;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomListView), typeof(CustomListViewRenderer))]
namespace XamFormsPerf.Droid.CustomRenderers
{
    public class CustomListViewRenderer : ListViewRenderer
    {
        int _itemCount;
        int _currentItem = 20;
        ListView _list;
        CustomListView _element;

        public CustomListViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element != null)
            {
                _element = Element as CustomListView;
                _list = Control as ListView;

                (_element as ICustomListView).ScrollToEnd = async () => await ScrollToBottom();
            }
        }

        private async Task ScrollToBottom()
        {
            _itemCount = _element.ItemsSource.Cast<object>().Count();

            _list.SetSelection(_currentItem++);

            for (int i = _currentItem; i < _itemCount; ++i)
            {
                await Task.Delay(1);
                _list.SetSelection(_currentItem++);
            }
        }
    }
}