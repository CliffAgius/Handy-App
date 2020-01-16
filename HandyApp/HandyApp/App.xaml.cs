using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace HandyApp
{
    public partial class App : Application
    {
        public static IDevice device { get; set; }

        public App()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTQ0MTgxQDMxMzcyZTMyMmUzMHBFSmJzcDM1Z0podkVNYWl2bnYweEh2eE5YVytlN1RnanhabHhvN1lRbW89");

            Device.SetFlags(new[] { 
                "SwipeView_Experimental"
            });

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
