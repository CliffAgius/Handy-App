using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace HandyApp.ViewModels
{
    public class HandControlViewModel : BaseViewModel
    {
        public ICommand ButtonCommand;

        public HandControlViewModel()
        {
            if (App.BTService.Device is null)
            {
                //We have no BT COnnection yet so goto the the Connection page first...
                AppShell.Current.GoToAsync("BTConnection");
            }

            ButtonCommand = new Command<string>(ActionButtonCommand);
        }

        private async void ActionButtonCommand(string UARTCommand)
        {
            await App.BTService.SendUARTCommand(UARTCommand).ConfigureAwait(false);
        }
    }
}
