using Acr.UserDialogs;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace HandyApp.ViewModels
{
    public class HandControlViewModel : BaseViewModel
    {
        public ICommand ButtonCommand;
        IUserDialogs Dialogs;

        public HandControlViewModel(IUserDialogs dialogs)
        {
            Dialogs = dialogs;
            if (App.BTService.Device is null)
            {
                Dialogs.Alert("Not Connected please connect first...");
                //We have no BT Connection yet so goto the the Connection page first...
                AppShell.Current.GoToAsync("BTConnection");
            }

            ButtonCommand = new Command<string>(ActionButtonCommand);
        }

        private async void ActionButtonCommand(string UARTCommand)
        {
            Dialogs.Toast($"Actioning {UARTCommand} move command...");
            await App.BTService.SendUARTCommand(UARTCommand).ConfigureAwait(false);
        }
    }
}
