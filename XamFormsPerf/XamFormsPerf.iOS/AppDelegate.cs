using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using System.Runtime.InteropServices;

namespace XamFormsPerf.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Calabash.Start();
            PerfLog.MeasureStart("First Page Load");
            PerfLog.Measure("App Init", () => global::Xamarin.Forms.Forms.Init());
            PerfLog.Measure("App Load", () => LoadApplication(new App()));

            return base.FinishedLaunching(app, options);
        }

        [Export("exitApp:")]
        public NSString ExitApp(NSString value)
        {
            Environment.Exit(0);
            return new NSString();
        }

        [Export("testsSummary:")]
        public NSString TestsSummary(NSString arg)
        {
            return new NSString(PerfLog.Summary());
        }
    }
}
