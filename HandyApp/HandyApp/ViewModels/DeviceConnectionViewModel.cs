using Acr.UserDialogs;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HandyApp.ViewModels
{
    public class DeviceConnectionViewModel : BaseViewModel
    {
        private IDevice Hand;
        private IAdapter Adapter;
        private IUserDialogs Dialogs;
        public ICharacteristic writeCharacteristic;
        public ICharacteristic readCharacteristic;
        public ICommand UARTCommand { get; }

        public string DeviceName { get; set; }
        private Guid DeviceId { get; set; }


        //public ObservableList<String> BTDataRcvd { get; private set; } = new ObservableList<String>();

        Guid serviceGuid = Guid.Parse("6E400001-B5A3-F393-E0A9-E50E24DCCA9E");
        Guid writeGuid = Guid.Parse("6e400002-b5a3-f393-e0a9-e50e24dcca9e");
        Guid readGuid = Guid.Parse("6e400003-b5a3-f393-e0a9-e50e24dcca9e");


        public DeviceConnectionViewModel(IUserDialogs dialogs)
        {
            try
            {
                Dialogs = dialogs;
                Hand = App.device;
                DeviceId = Hand.Id;
                DeviceName = Hand.Name;
                UARTCommand = new AsyncCommand<string>(SendUARTCommand);

                Task.Run(() => Connect());
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
                UARTString += "\n\r";

                byte[] bytes = Encoding.ASCII.GetBytes(UARTString);
                await writeCharacteristic.WriteAsync(bytes).ConfigureAwait(false);
                //var test = readCharacteristic.ReadAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error while sending command - {ex.Message}");
            }
        }

        private void ReadCharacteristic_ValueUpdated(object sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
        {
            if (readCharacteristic.CanRead)
            {
                var test = readCharacteristic.ReadAsync();
            }
        }

        private async Task Connect()
        {
            try
            {
                Adapter = CrossBluetoothLE.Current.Adapter;
                await Adapter.ConnectToDeviceAsync(Hand);
                var service = await Hand.GetServiceAsync(serviceGuid).ConfigureAwait(false);

                var characteristics = await service.GetCharacteristicsAsync();


                writeCharacteristic = await service.GetCharacteristicAsync(writeGuid).ConfigureAwait(false);
                readCharacteristic = await service.GetCharacteristicAsync(readGuid).ConfigureAwait(false);

                readCharacteristic.ValueUpdated += ReadCharacteristic_ValueUpdated;
                await readCharacteristic.StartUpdatesAsync();
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error while sending command - {ex.Message}");
            }
        }
    }
}

//    Hand.Connect();

//    // this will watch the connection states to the device
//    Hand.WhenStatusChanged().Subscribe(async connectionState =>
//    {
//        Dialogs.Toast($"Connection state has changed - {connectionState.ToString()}");
//        try
//        {
//            Hand = await CrossBleAdapter.Current.GetKnownDevice(DeviceId).FirstOrDefaultAsync();
//        }
//        catch
//        {
//            Dialogs.Toast("Unable to connect...");
//        }
//    }, ex =>
//    {
//        Dialogs.Toast($"Status Change Error - {ex.Message}", TimeSpan.FromSeconds(3));
//    });

//    //Command that sends the UART data to the Hand...
//    UARTCommand = ReactiveCommand.Create<string>(UARTString =>
//    {
//        UARTString += "\n\r";

//        byte[] bytes = Encoding.ASCII.GetBytes(UARTString);

//        Hand.GetKnownCharacteristics(serviceGuid, writeGuid)
//        .ObserveOn(RxApp.MainThreadScheduler)
//        .Subscribe(characteristic =>
//        {
//            if (characteristic.CanWrite())
//            {
//                characteristic.WriteWithoutResponse(bytes).Subscribe(result =>
//                {
//                    // you don't really need to do anything with the result
//                    string str = Encoding.Default.GetString(result.Data);
//                    Dialogs.Toast($"Message Sent - {str}");
//                },
//                exception =>
//                {
//                    Dialogs.Toast($"Write Error - {exception.Message}");
//                });
//            }
//        },
//        ex =>
//        {
//            Dialogs.Toast($"Write Error - {ex.Message}", TimeSpan.FromSeconds(3));
//        });
//    });

//    //Recieve Data from Hand...
//    Hand.ConnectHook(serviceGuid, readGuid).Subscribe(result =>
//    {
//        string str = Encoding.Default.GetString(result.Data);
//        Dialogs.Toast($"Message Recv'd - {str}", TimeSpan.FromSeconds(3));

//        BTDataRcvd.Insert(0, str);
//    }, ex =>
//    {
//        Dialogs.Toast($"Read Error - {ex.Message}", TimeSpan.FromSeconds(3));
//    });

//    PairToDevice = ReactiveCommand.Create(() =>
//    {
//        if (!Hand.Features.HasFlag(DeviceFeatures.PairingRequests))
//        {
//            Dialogs.Toast("Pairing is not supported on this platform");
//        }
//        else if (Hand.PairingStatus == PairingStatus.Paired)
//        {
//            Dialogs.Toast("Device is already paired");
//        }
//        else
//        {
//            Hand.PairingRequest()
//                .Subscribe(x =>
//                {
//                    var txt = x ? "Device Paired Successfully" : "Device Pairing Failed";
//                    Dialogs.Toast(txt);
//                }, ex =>
//                {
//                    Dialogs.Toast($"Pairing Error - {ex.Message}", TimeSpan.FromSeconds(3));
//                });
//        }
//    });