using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Diagnostics;
using Java.Interop;
using Android.Util;

namespace XamFormsPerf.Droid
{
    [Activity(Label = "XamFormsPerf", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            PerfLog.MeasureStart("First Page Load");
            PerfLog.Measure("App Init", () => global::Xamarin.Forms.Forms.Init(this, bundle));
            PerfLog.Measure("App Load", () => LoadApplication(new App()));
        }

        [Export("ExitApp")]
        public void ExitApp()
        {
            Log.Info("UITest", "Finish");
            FinishAndRemoveTask();
            //Android.OS.Process.KillProcess(Android.OS.Process.MyPid()); // This kills not only app, but also tests
        }

        [Export("TestsSummary")]
        public string TestsSummary()
        {
            return PerfLog.Summary();
        }
    }
}

