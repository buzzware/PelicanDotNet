﻿using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;

namespace PelicanExample.Android;

[Activity(
    Label = "PelicanExample.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            //.UseReactiveUI()
            ;
    }
    
    public override async void OnBackPressed()
    {
        if (!(await AppCommon.Router.HandleHardwareBackButton()))
            base.OnBackPressed(); // This will terminate the app.
    }    
}
