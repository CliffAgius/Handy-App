<<<<<<< HEAD
﻿using HandyApp.Views;
using Plugin.BluetoothLE;
using System.Threading.Tasks;
=======
﻿using Plugin.BLE.Abstractions.Contracts;
>>>>>>> bt-test
using Xamarin.Forms;

namespace HandyApp
{
    public partial class App : Application
    {
        public static IDevice device { get; set; }

        public App()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTcyNDY0QDMxMzcyZTMzMmUzMGpQaEhtTXRBQnNreTdHZ2dDNmRJQWdpZlY2Y01lK3VDZXp4MHl5NHBZb2s9");

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
