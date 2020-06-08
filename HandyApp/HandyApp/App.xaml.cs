using HandyApp.Models;
using HandyApp.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace HandyApp
{
    public partial class App : Application
    {
        public static BlueToothService BTService { get; set; }
        public static ObservableCollection<Grip> Grips { get; set; }

        const string AppCenteriOS = "APPCENTER_IOS";
        const string AppCenterAndroid = "APPCENTER_ANDROID";

        public App()
        {

            Xamarin.Forms.Device.SetFlags(new[] { 
                "SwipeView_Experimental",
                "CarouselView_Experimental",
                "IndicatorView_Experimental"
            });

            //Config the Bluetooth Service
            BTService = new BlueToothService();

            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // Handle when your app starts

#if !DEBUG
            AppCenter.Start($"ios={AppCenteriOS};" +
                $"android={AppCenterAndroid};", 
                typeof(Analytics), 
                typeof(Crashes),
                typeof(Distribute));
#endif
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
