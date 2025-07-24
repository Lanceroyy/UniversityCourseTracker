using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.View.InputMethod;
using Android.Views;

namespace C971App
{
    [Activity(Theme = "@style/Maui.SplashTheme",
          MainLauncher = true,
          ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
          WindowSoftInputMode = SoftInput.AdjustResize
        )]

    public class MainActivity : MauiAppCompatActivity
    {
    }

}
