using Acr.UserDialogs;
using HandyApp.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace HandyApp.ViewModels
{
    public class UARTControlViewModel : BaseViewModel
    {
        private IUserDialogs Dialogs;
        public ICommand UARTCommand { get; }

        public ObservableRangeCollection<string> UARTDataRcvd { get; private set; } = new ObservableRangeCollection<string>();

        public UARTControlViewModel(IUserDialogs dialogs)
        {
            try
            {
                Dialogs = dialogs;
                UARTCommand = new AsyncCommand<string>(SendUARTCommand);
                SetupBT().ConfigureAwait(false);                
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry there is a fault - {ex.Message}");
            }
        }

        private async Task SetupBT()
        {
            try
            {
                await App.BTService.Connect();
                App.BTService.BTDataRcvd.CollectionChanged += BTDataRcvd_CollectionChanged;
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error while setting up the BT connection - {ex.Message}");
            }
        }

        private void BTDataRcvd_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                UARTDataRcvd.ReplaceRange(App.BTService.BTDataRcvd);
                OnPropertyChanged(nameof(UARTDataRcvd));
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error while recieving the command - {ex.Message}");
            }
            
        }

        private async Task SendUARTCommand(string UARTString)
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