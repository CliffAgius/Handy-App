using Acr.UserDialogs;
using MvvmHelpers;
using System;
using System.Windows.Input;

namespace HandyApp.ViewModels
{
    public class DeviceConnectionViewModel : BaseViewModel
    {
        //private IDevice Hand;
        //private IUserDialogs Dialogs;
        public ICommand PairToDevice { get; }
        public ICommand UARTCommand { get; }

        public string DeviceName { get; set; }
        private Guid DeviceId { get; set; }


        //public ObservableList<String> BTDataRcvd { get; private set; } = new ObservableList<String>();

        Guid serviceGuid = Guid.Parse("6E400001-B5A3-F393-E0A9-E50E24DCCA9E");
        Guid writeGuid = Guid.Parse("6E400002-B5A3-F393-E0A9-E50E24DCCA9E");
        Guid readGuid = Guid.Parse("6E400003-B5A3-F393-E0A9-E50E24DCCA9E");


        public DeviceConnectionViewModel(IUserDialogs dialogs)
        {
            //try
            //{
            //    Dialogs = dialogs;
            //    Hand = App.device;
            //    DeviceId = Hand.Uuid;
            //    DeviceName = Hand.Name;

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
            //}
            //catch (Exception ex)
            //{
            //    Dialogs.Alert($"Sorry there is a fault - {ex.Message}");
            //}
        }

    }
}
