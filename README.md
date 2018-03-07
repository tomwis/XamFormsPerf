# Xamarin.Forms Performance Checker

This app was created to check changes in performance between Xamarin.Forms versions. For now it has 18 tests that run automatically after starting the app:
- Time of Forms.Init()
- Time of LoadApplication()
- Time to first page (to OnAppearing)
- Time of creating and rendering basic controls: Label, Button, BoxView, Image
- Time of creating and scrolling through ListView with TextCells, ImageCells, SwitchCells, EntryCells, simple and complex ViewCells
- Time of rendering different layouts: 2 different nested StackLayouts, 3 nested Grid layouts where each page has only Grids with sizes as Stars, Auto or constant values

# How to use?

If you want to use it yourself, just download it, set Xamarin.Forms version in nuget manager and run the app.

There is also automated UI Test (for Android and iOS). UI Test can be performed a few times in a loop. After it's done it saves average results to csv.

In repo you can also find Cake Build script. It can be used to automatically change Xamarin.Forms nuget package version, restore nugets, rebuild projects and run UI Tests on devices. It can be performed automatically for many Forms versions in a loop.

# Results Preview

Results of current tests can be viewed on this website: http://xamformsperf.azurewebsites.net
