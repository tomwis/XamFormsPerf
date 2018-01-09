using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XamFormsPerf.UITests
{
    public class AppInitializer
    {
        static readonly string _bundleName = "com.companyname.XamFormsPerf";

        public static IApp StartApp(Platform platform)
        {
            var config = "Release";
#if DEBUG
            config = "Debug";
#endif

            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .ApkFile($@"..\..\..\XamFormsPerf\XamFormsPerf.Android\bin\{config}\{_bundleName}.apk")
                    .StartApp();
            }

            return ConfigureApp
                .iOS
                .InstalledApp(_bundleName)
                .StartApp();
        }
    }
}

