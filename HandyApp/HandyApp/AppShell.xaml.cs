using HandyApp.Helpers;
using HandyApp.Views;
using System;
using System.Collections.Generic;
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

            Routing.RegisterRoute("BTConnection", typeof(ListAdaptersView));
            Routing.RegisterRoute("UARTControl", typeof(UARTControlView));

        }

        internal async Task CloseFlyoutAsync()
        {
            FlyoutIsPresented = false;
            await Task.Delay(TimeFlyoutCloses);
        }
    }
}
