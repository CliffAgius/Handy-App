using Acr.UserDialogs;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HandyApp.ViewModels
{
    public class UARTControlViewModel : BaseViewModel
    {
        private IUserDialogs Dialogs;
        public ICommand UARTCommand { get; }
        public ICommand ClearCommand { get; }

        public string UARTString { get; set; }

        public UARTControlViewModel(IUserDialogs dialogs)
        {
            try
            {
                Dialogs = dialogs;
                UARTCommand = new AsyncCommand<string>(HandleSendUARTCommand);
                ClearCommand = new Command(HandleClearCommand);
                SetupBT().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry there is a fault - {ex.Message}");
            }
        }

        private void HandleClearCommand()
        {
            UARTString = "";
        }

        private async Task SetupBT()
        {
            try
            {
                await App.BTService.Connect();
                App.BTService.RcvdDataHandler += BTService_RcvdDataHandler;
                //App.BTService. += BTService_PropertyChanged;
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error while setting up the BT connection - {ex.Message}");
            }
        }

        private void BTService_RcvdDataHandler(object sender, EventArgs e)
        {
            try
            {
                UARTString += App.BTService.RcvdDataString;
                OnPropertyChanged(nameof(UARTString));
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error while recieving the command - {ex.Message}");
            }
        }

        private async Task HandleSendUARTCommand(string UARTString)
        {
            try
            {
                await App.BTService.SendUARTCommand(UARTString);
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error while sending the command - {ex.Message}");
            }
        }
    }
}