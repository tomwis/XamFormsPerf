using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamFormsPerf.Pages.Controls;
using XamFormsPerf.Pages.Layouts;
using XamFormsPerf.Pages.ListView;

namespace XamFormsPerf.Pages
{
    public partial class MainPage : ContentPage
    {
        bool _automated = true;
        const int _loops = 1;
        int _currentLoop = 0;
        int _currentTest = 0;
        List<Type> _tests = new List<Type>
        {
            // Controls
            typeof(BoxViewTest),
            typeof(LabelTest),
            typeof(ButtonTest),
            typeof(ImageTest),

            // Layouts
            typeof(NestedStackLayoutTest),
            typeof(NestedGridLikeStackLayoutTest),
            typeof(NestedGridStarSizeTest),
            typeof(NestedGridAutoSizeTest),
            typeof(NestedGridConstSizeTest),

            // ListView
            typeof(ListViewTextCellTest),
            typeof(ListViewImageCellTest),
            typeof(ListViewEntryCellTest),
            typeof(ListViewSwitchCellTest),
            typeof(ListViewSimpleViewCellTest),
            typeof(ListViewExtendedViewCellTest),
        };

        public MainPage()
        {
            InitializeComponent();

            TestList.ItemsSource = _tests;
            TestList.ItemSelected += TestList_ItemSelected;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            PerfLog.MeasureStop("First Page Load");

            await Task.Delay(100);
            await AutomatedTests();
        }

        async Task AutomatedTests()
        {
            if (_automated)
            {
                if (_currentTest >= _tests.Count)
                {
                    _currentTest = 0;
                    ++_currentLoop;

                    if (_currentLoop >= _loops)
                    {
                        _automated = false;
                        TestList.IsEnabled = true;

                        MarkTestsAsFinishedForUITests();
                        Summary.Text = "Summary: " + Environment.NewLine;
                        Summary.Text += PerfLog.Summary();
                    }
                }

                if (_currentLoop < _loops)
                {
                    Summary.Text = $"Tests in progress ({_currentLoop + 1}/{_loops})";

                    TestList.IsEnabled = false;
                    await GoToTest(_tests[_currentTest++]);
                }
            }
        }

        void MarkTestsAsFinishedForUITests()
        {
            Panel.Children.Add(new BoxView { AutomationId = "FinishedTestsId" });
        }
        
        private async void TestList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (TestList.SelectedItem != null)
            {
                var type = TestList.SelectedItem as Type;
                await GoToTest(type);

                TestList.SelectedItem = null;
            }
        }

        async Task GoToTest(Type type)
        {
            PerfLog.MeasureStart(type.Name);
            await Navigation.PushAsync(Activator.CreateInstance(type, _automated) as Page, false);
        }
    }
}
