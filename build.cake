///////////////////////////////////////////////////////////////////////////////
// ADDINS
///////////////////////////////////////////////////////////////////////////////
#addin Cake.FileHelpers
#addin Cake.Xamarin
#tool nuget:?package=NUnit.Runners&version=2.6.4

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var formsVersions = Argument("formsVersions", "2.4.0.280;2.4.0.18342;2.4.0.38779;2.4.0.74863;2.5.0.77107;2.5.0.121934;2.5.0.122203");
var currentFormsVersion = "";

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("clean")
    .Does (() => 
    {
        CleanDirectories ("./XamFormsPerf/**/bin");
        CleanDirectories ("./XamFormsPerf/**/obj");
    });

Task("replace-forms-version")
    .Does(() =>
    {
        if(!string.IsNullOrWhiteSpace(currentFormsVersion))
        {
            Information($"Replacing forms version to {currentFormsVersion}.");

            var regexPattern = @"id=\""Xamarin\.Forms\""\s{1,}version=\""[0-9.]+\""";
            var replaceText = $@"id=""Xamarin.Forms"" version=""{currentFormsVersion}""";

            ReplaceRegexInFiles("./XamFormsPerf/**/packages.config", regexPattern, replaceText);

            Information($"Replacing forms version to {currentFormsVersion} - done.");
        }
        else
        {
            Warning($"[warning] currentFormsVersion variable is empty - no replacing of forms version will happen. Any further build will run with current version in packages.config.");
        }
    });

Task("restore-android")
    .Does(() => 
    {
        RestoreNuget("./XamFormsPerf/XamFormsPerf/XamFormsPerf.csproj");
        RestoreNuget("./XamFormsPerf/XamFormsPerf.Android/XamFormsPerf.Android.csproj");
    });

Task("restore-ios")
    .Does(() => 
    {
        RestoreNuget("./XamFormsPerf/XamFormsPerf/XamFormsPerf.csproj");
        RestoreNuget("./XamFormsPerf/XamFormsPerf.iOS/XamFormsPerf.iOS.csproj");
    });

void RestoreNuget(string projectPath)
{
    Information($"Restoring nuget packages for {projectPath}.");

    var nugetSettings = new NuGetRestoreSettings
    {
        ArgumentCustomization = args => args.Append("-PackagesDirectory ./packages")
    };
    NuGetRestore (projectPath, nugetSettings);

    Information($"Restoring nuget packages for {projectPath} - done.");
}

Task("build-android")
    .IsDependentOn("clean")
    .IsDependentOn("restore-android")
    .Does(() => 
    {
        MSBuild("./XamFormsPerf/XamFormsPerf.Android/XamFormsPerf.Android.csproj",
                c => c.SetConfiguration("Release")
                .WithTarget("SignAndroidPackage")
                .SetVerbosity(Verbosity.Minimal)
        );
    });

Task("build-ios")
    .IsDependentOn("clean")
    .IsDependentOn("restore-ios")
    .Does(() => 
    {
        MSBuild("./XamFormsPerf.sln",
                c => c.SetConfiguration("Release")
                .WithProperty("Platform", "iPhone")
                .SetVerbosity(Verbosity.Minimal)
                //.WithProperty("BuildIPA", "True")
                );
    });

Task("build-uitests")
    .Does(() =>
    {
        MSBuild("./XamFormsPerf.UITests/XamFormsPerf.UITests.csproj",
                c => c.SetConfiguration("Release")
                .SetVerbosity(Verbosity.Minimal)
        );
    });

Task("uitests-android")
    .IsDependentOn("build-android")
    .IsDependentOn("build-uitests")
    .Does(() =>
    {
        NUnit("./XamFormsPerf.UITests/bin/Release/XamFormsPerf.UITests.dll", new NUnitSettings
        {
            ArgumentCustomization = args => args.Append("-run=\"XamFormsPerf.UITests.Tests(Android).AppLaunches\"")
        });
    });

Task("uitests-ios")
    .IsDependentOn("build-ios")
    .IsDependentOn("build-uitests")
    .Does(() =>
    {
        StartProcess("ios-deploy", new ProcessSettings 
        {
            Arguments = "--uninstall_only --bundle_id com.apple.test.DeviceAgent-Runner"
        });

        StartProcess("ios-deploy", new ProcessSettings 
        {
            Arguments = "--uninstall --bundle ./XamFormsPerf/XamFormsPerf.iOS/bin/iPhone/Release/XamFormsPerf.iOS.app"
        });

        NUnit("./XamFormsPerf.UITests/bin/Release/XamFormsPerf.UITests.dll", new NUnitSettings
        {
            ArgumentCustomization = args => args.Append("-run=\"XamFormsPerf.UITests.Tests(iOS).AppLaunches\"")
        });
    });


Task("uitests-loop")
    .Does(() =>
    {
        if(string.IsNullOrWhiteSpace(formsVersions))
        {
            Information("No forms versions defined - using current one to run tests, You can define them in formsVersions argument, separated by semicolon.");

            RunTarget("uitests-android");
            RunTarget("uitests-ios");
        }
        else
        {
            Information($"Tests will run with each version from formsVersions arguemnt: {formsVersions}.");

            var versions = formsVersions.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            Information($"Forms versions count: {versions.Length}.");

            for(int i = 0; i < versions.Length; ++i)
            {
                currentFormsVersion = versions[i];

                Information($"Starting tasks for forms version: {currentFormsVersion}.");

                RunTarget("replace-forms-version");
                RunTarget("uitests-android");
                RunTarget("uitests-ios");
            }
        }
    });

RunTarget(target);