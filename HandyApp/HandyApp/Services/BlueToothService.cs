using MvvmHelpers;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HandyApp.Services
{
    public class BlueToothService
    {
        Guid serviceGuid = Guid.Parse("6E400001-B5A3-F393-E0A9-E50E24DCCA9E");
        Guid writeGuid = Guid.Parse("6e400002-b5a3-f393-e0a9-e50e24dcca9e");
        Guid readGuid = Guid.Parse("6e400003-b5a3-f393-e0a9-e50e24dcca9e");

        private ICharacteristic writeCharacteristic;
        private ICharacteristic readCharacteristic;

        public event EventHandler RcvdDataHandler;
        
        public string RcvdDataString { get; set; }

        public BlueToothService()
        {

        }

        public IAdapter Adapter { get; set; }
        public IDevice Device { get; set; }

        public async Task Connect()
        {
            try
            {
                Adapter = CrossBluetoothLE.Current.Adapter;
                await Adapter.ConnectToDeviceAsync(Device);

                var service = await Device.GetServiceAsync(serviceGuid).ConfigureAwait(false);

                writeCharacteristic = await service.GetCharacteristicAsync(writeGuid).ConfigureAwait(false);
                readCharacteristic = await service.GetCharacteristicAsync(readGuid).ConfigureAwait(false);

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    readCharacteristic.ValueUpdated += ReadCharacteristic_ValueUpdated;
                    await readCharacteristic.StartUpdatesAsync();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Sorry an error while sending command - {ex.Message}");
            }
        }

        public async Task Disconnect()
        {
            try
            {
                await Adapter.DisconnectDeviceAsync(Device).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Sorry there was a problem trying to disconnect... - {ex.Message}");
            }
        }

        public async Task SendUARTCommand(string UARTString)
        {
            try
            {
                if (writeCharacteristic is null)
                {
                    Debug.WriteLine("The writeCharacteristic is Null so we can't write...");
                    return;
                }
                UARTString += "\n\r";

                byte[] bytes = Encoding.ASCII.GetBytes(UARTString);
                await writeCharacteristic.WriteAsync(bytes).ConfigureAwait(false);
                //var test = readCharacteristic.ReadAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Sorry an error while sending command - {ex.Message}");
            }
        }

        private void ReadCharacteristic_ValueUpdated(object sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
        {
            try
            {
                if (e.Characteristic?.Value is object)
                {
                    RcvdDataString = Encoding.UTF8.GetString(e.Characteristic.Value);
                    RcvdDataHandler.Invoke(this, null);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error recieving data - {ex.Message}");
            }
        }
    }
}
