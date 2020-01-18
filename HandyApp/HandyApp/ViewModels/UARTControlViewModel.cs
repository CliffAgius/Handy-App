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
        //public  IDevice Device;
        //private IAdapter Adapter;
        private IUserDialogs Dialogs;
        //public ICharacteristic writeCharacteristic;
        //public ICharacteristic readCharacteristic;
        public ICommand UARTCommand { get; }

        public ObservableRangeCollection<string> BTDataRcvd { get; private set; } = new ObservableRangeCollection<string>();
        private string rcvdDataString;

        //Guid serviceGuid = Guid.Parse("6E400001-B5A3-F393-E0A9-E50E24DCCA9E");
        //Guid writeGuid = Guid.Parse("6e400002-b5a3-f393-e0a9-e50e24dcca9e");
        //Guid readGuid = Guid.Parse("6e400003-b5a3-f393-e0a9-e50e24dcca9e");


        public UARTControlViewModel(IUserDialogs dialogs)
        {
            try
            {
                Dialogs = dialogs;
                UARTCommand = new AsyncCommand<string>(SendUARTCommand);
                Task.Run(() => App.BTService.Connect());
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry there is a fault - {ex.Message}");
            }
        }

        private async Task SendUARTCommand(string UARTString)
        {
            try
            {
                //UARTString += "\n\r";

                //byte[] bytes = Encoding.ASCII.GetBytes(UARTString);
                await App.BTService.SendUARTCommand(UARTString);
                
                //await writeCharacteristic.WriteAsync(bytes).ConfigureAwait(false);
                //var test = readCharacteristic.ReadAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error while sending command - {ex.Message}");
            }
        }

        private void ReadCharacteristic_ValueUpdated(object sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (e.Characteristic?.Value is object)
                {
                    var rcvdValue = Encoding.UTF8.GetString(e.Characteristic.Value);
                    rcvdDataString += rcvdValue;
                    if (e.Characteristic.Value[e.Characteristic.Value.Length -1] == 10)
                    {
                        BTDataRcvd.Add(rcvdDataString);
                        rcvdDataString = "";
                    }
                }
            });
        }

        //private async Task Connect()
        //{
        //    try
        //    {
        //        Adapter = CrossBluetoothLE.Current.Adapter;
        //        await Adapter.ConnectToDeviceAsync(Device);
        //        var service = await Device.GetServiceAsync(serviceGuid).ConfigureAwait(false);

        //        var characteristics = await service.GetCharacteristicsAsync();


        //        writeCharacteristic = await service.GetCharacteristicAsync(writeGuid).ConfigureAwait(false);
        //        readCharacteristic = await service.GetCharacteristicAsync(readGuid).ConfigureAwait(false);

        //        readCharacteristic.ValueUpdated += ReadCharacteristic_ValueUpdated;
        //        await readCharacteristic.StartUpdatesAsync().ConfigureAwait(false);
        //    }
        //    catch (Exception ex)
        //    {
        //        Dialogs.Alert($"Sorry an error while sending command - {ex.Message}");
        //    }
        //}
    }
}