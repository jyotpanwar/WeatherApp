using Android.App;
using Android.Content.PM;
using MvvmCross.Forms.Platforms.Android.Views;
using MvvmCross.Forms.Platforms.Android.Core;
using Android.OS;
using Xamarin.Essentials;

namespace WeatherApp.Droid
{
    [Activity(Label = "WeatherApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]

    public class RootActivity : MvxFormsAppCompatActivity<MvxFormsAndroidSetup<Core.App, App>, Core.App, App>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Platform.Init(this.Application);
        }
    }
}