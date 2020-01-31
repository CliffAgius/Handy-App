using HandyApp.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HandyApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public static readonly TimeSpan TimeFlyoutCloses = TimeSpan.FromSeconds(0.5f);

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("HandControl", typeof(HandControlView));
            Routing.RegisterRoute("BTConnection", typeof(BTConnectionView));
            Routing.RegisterRoute("UARTControl", typeof(UARTControlView));
            Routing.RegisterRoute("MuscleSensorCheck", typeof(MuscleSensorCheckView));
        }

        internal async Task CloseFlyoutAsync()
        {
            FlyoutIsPresented = false;
            await Task.Delay(TimeFlyoutCloses);
        }
    }
}
