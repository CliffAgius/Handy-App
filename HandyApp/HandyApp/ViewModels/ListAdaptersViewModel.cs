//using Plugin.BluetoothLE;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Acr.UserDialogs;
using Xamarin.Forms;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System.Threading.Tasks;
using System.Diagnostics;
using MvvmHelpers;
using Xamarin.Essentials;
using Plugin.BLE.Abstractions.EventArgs;
using System.Linq;
using Plugin.Permissions.Abstractions;
using System.Threading;
using Plugin.Permissions;

namespace HandyApp.ViewModels
{
    public class ListAdaptersViewModel : BaseViewModel
    {
        IUserDialogs Dialogs;
        IBluetoothLE ble;
        IAdapter Adapter;
        public bool IsStateOn => ble.IsOn;
        public bool IsRefreshing => Adapter.IsScanning;

        public ICommand StartScanBtn { get; private set; }
        public ICommand StopScanBtn { get; private set; }

        public BluetoothState BluetoothState { get; set; }

        public ObservableCollection<DeviceListItemViewModel> Devices { get; set; } = new ObservableCollection<DeviceListItemViewModel>();

        public ListAdaptersViewModel(IUserDialogs dialogs)
        {
            Dialogs = dialogs;
            ble = CrossBluetoothLE.Current;
            Adapter = CrossBluetoothLE.Current.Adapter;
            BluetoothState = ble.State;
            IsScanning = Adapter.IsScanning.ToString();

            //Events
            Adapter.DeviceDiscovered += OnDeviceDiscovered;
            ble.StateChanged += OnStateChanged;

            //Set-up the commands...
            StartScanBtn = new Command(async () => await StartScan().ConfigureAwait(false));
            StopScanBtn = new Command(async () => await StopScan().ConfigureAwait(false));
        }

        private void OnStateChanged(object sender, BluetoothStateChangedArgs e)
        {
            OnPropertyChanged(nameof(BluetoothState));
            OnPropertyChanged(nameof(StateText));
            Debug.WriteLine($"The bluetooth state changed to {e.NewState}");
            Dialogs.Toast($"BT Connection state has changed - {BluetoothState.ToString()}");
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs args)
        {
            AddOrUpdateDevice(args.Device);
        }

        private void AddOrUpdateDevice(IDevice device)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var vm = Devices.FirstOrDefault(d => d.Device.Id == device.Id);
                if (vm != null)
                {
                    vm.Update();
                }
                else
                {
                    Devices.Add(new DeviceListItemViewModel(device));
                }
            });
        }


        private async Task StopScan()
        {
            try
            {
                if (Adapter.IsScanning)
                {
                    IsBusy = false;
                    OnPropertyChanged(nameof(IsScanning));
                    await Adapter.StopScanningForDevicesAsync();
                    Debug.WriteLine("adapter.StopScanningForDevices()");
                    Dialogs.Toast($"Stopped Scanning For Devices... IsScanning - {Adapter.IsScanning}");
                }
            }
            catch (Exception ex)
            {
                Dialogs.Toast($"Error - {ex.Message}");
            }
        }

        private async Task StartScan()
        {
            try
            {
                if (Xamarin.Forms.Device.RuntimePlatform == Device.Android)
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                    if (status != PermissionStatus.Granted)
                    {
                        var permissionResult = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

                        if (permissionResult.First().Value != PermissionStatus.Granted)
                        {
                            Dialogs.Toast("Permission denied. Not scanning.");
                            CrossPermissions.Current.OpenAppSettings();
                            return;
                        }
                    }
                }

                if (IsStateOn && (!Devices.Any()) && !IsRefreshing)
                {
                    ScanForDevices();
                }
            }
            catch (Exception ex)
            {
                Dialogs.Toast($"Error - {ex.Message}");
            }
        }

        private async void ScanForDevices()
        {
            Devices.Clear();

            foreach (var connectedDevice in Adapter.ConnectedDevices)
            {
                //update rssi for already connected evices (so tha 0 is not shown in the list)
                try
                {
                    await connectedDevice.UpdateRssiAsync();
                }
                catch (Exception ex)
                {
                    Dialogs.Toast($"Failed to update RSSI for {connectedDevice.Name}");
                }

                AddOrUpdateDevice(connectedDevice);
            }

            Adapter.ScanMode = ScanMode.LowLatency;
            await Adapter.StartScanningForDevicesAsync();
        }

        private string GetStateText()
        {
            switch (ble.State)
            {
                case BluetoothState.Unknown:
                    return "Unknown BLE state.";
                case BluetoothState.Unavailable:
                    return "BLE is not available on this device.";
                case BluetoothState.Unauthorized:
                    return "You are not allowed to use BLE.";
                case BluetoothState.TurningOn:
                    return "BLE is warming up, please wait.";
                case BluetoothState.On:
                    return "BLE is on.";
                case BluetoothState.TurningOff:
                    return "BLE is turning off. That's sad!";
                case BluetoothState.Off:
                    return "BLE is off. Turn it on!";
                default:
                    return "Unknown BLE state.";
            }
        }

        public string Title { get; private set; }
        public string IsScanning { get; private set; }
        private string _stateText;
        public string StateText
        {
            get
            {
                _stateText = GetStateText();
                return _stateText;
            }
        }
    }
}