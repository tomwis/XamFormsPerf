using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamFormsPerf.CustomControls;
using XamFormsPerf.iOS.CustomRenderers;
using XamFormsPerf.Pages.ListView;

[assembly: ExportRenderer(typeof(CustomListView), typeof(CustomListViewRenderer))]
namespace XamFormsPerf.iOS.CustomRenderers
{
    public class CustomListViewRenderer : ListViewRenderer
    {
        int _itemCount;
        int _currentItem = 20;
        UITableView _table;
        CustomListView _element;
        TaskCompletionSource<bool> _scrollToBottomTask;

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if(Control != null && Element != null)
            {
                _element = Element as CustomListView;
                _itemCount = Element.ItemsSource.Cast<object>().Count();
                _table = Control as UITableView;
                _table.Delegate = null;
                _table.Scrolled += _table_Scrolled;

                (_element as ICustomListView).ScrollToEnd = async () => await ScrollToBottom();
            }
        }

        private Task ScrollToBottom()
        {
            _itemCount = Element.ItemsSource.Cast<object>().Count();

            if (_itemCount < _currentItem) return Task.CompletedTask;

            if (_scrollToBottomTask != null && !(_scrollToBottomTask.Task.IsCanceled || _scrollToBottomTask.Task.IsCompleted || _scrollToBottomTask.Task.IsFaulted))
            {
                _scrollToBottomTask.SetCanceled();
            }

            _scrollToBottomTask = new TaskCompletionSource<bool>();

            ScrollTo(_currentItem++, false);

            return _scrollToBottomTask.Task;
        }

        private async void _table_Scrolled(object sender, EventArgs e)
        {
            if (_currentItem < _itemCount)
            {
                await Task.Delay(1);
                ScrollTo(_currentItem++, false);
            }
            else
            {
                _scrollToBottomTask.SetResult(true);
            }
        }

        private void ScrollTo(int itemIndex, bool animate)
        {
            var rowRect = _table.RectForRowAtIndexPath(NSIndexPath.FromRowSection(itemIndex, 0));
            var point = _table.Superview.ConvertPointToView(rowRect.Location, _table);
            _table.ScrollRectToVisible(new CoreGraphics.CGRect(rowRect.Location, rowRect.Size), animate);            
        }
    }
}