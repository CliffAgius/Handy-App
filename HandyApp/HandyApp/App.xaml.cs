using Xamarin.Forms;

namespace HandyApp
{
    public partial class App : Application
    {
        private static NavigableElement navigationRoot;

        public App()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTQ0MTgxQDMxMzcyZTMyMmUzMHBFSmJzcDM1Z0podkVNYWl2bnYweEh2eE5YVytlN1RnanhabHhvN1lRbW89");

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
