# Xamarin.Forms Performance Checker

This app was created to check changes in performance between Xamarin.Forms versions. For now it has 7 tests that run automatically after starting the app:
- Time of Forms.Init()
- Time of LoadApplication()
- Time to first page (to OnAppearing)
- Time of creating and rendering basic controls: Label, Button, BoxView, Image

If you want to use it yourself, just download it, set Xamarin.Forms version in nuget manager and run the app.

There is also automated UI Test (for now only for Android) which can be looped and saves results (and average results) to csv.

I will hopefully add more tests to the app, automate the process more and present results somewhere online.
