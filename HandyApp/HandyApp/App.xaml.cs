using HandyApp.Models;
using HandyApp.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace HandyApp
{
    public partial class App : Application
    {
        public static BlueToothService BTService { get; set; }
        public static ObservableCollection<Grip> Grips { get; set; }

        public App()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjAxNjQ3QDMxMzcyZTM0MmUzMENBWTdDaW9UNHpieHNZNkZwVmNhMjdkVEVDMHh0d01nYnBVU01GQkdGUEk9");

            Device.SetFlags(new[] { 
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
