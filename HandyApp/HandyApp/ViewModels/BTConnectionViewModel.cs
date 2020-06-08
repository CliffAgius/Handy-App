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
using Plugin.Permissions;
using MvvmHelpers.Commands;

namespace HandyApp.ViewModels
{
    public class BTConnectionViewModel : BaseViewModel
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

        public DeviceListItemViewModel SelectedDevice
        {
            get => null;
            set
            {
                if (value != null)
                {
                    HandleSelectedDevice(value);
                }

                OnPropertyChanged();
            }
        }

        private async void HandleSelectedDevice(DeviceListItemViewModel device)
        {
            //Set the selected Adapter and Device to the one we are going to use...
            App.BTService.Adapter = Adapter;
            App.BTService.Device = device.Device;
            //Stop scanning if it's still active...
            await StopScan().ConfigureAwait(false);
            //Navigate to the Connected Page...
            await Shell.Current.Navigation.PopToRootAsync();
        }

        public BTConnectionViewModel(IUserDialogs dialogs)
        {
            Dialogs = dialogs;
            ble = CrossBluetoothLE.Current;
            Adapter = CrossBluetoothLE.Current.Adapter;
            BluetoothState = ble.State;
            IsScanning = Adapter.IsScanning;

            //Events
            Adapter.DeviceDiscovered += OnDeviceDiscovered;
            ble.StateChanged += OnStateChanged;
            Adapter.ScanTimeoutElapsed += Adapter_ScanTimeoutElapsed;

            //Set-up the commands...
            StartScanBtn = new AsyncCommand(StartScan);
            StopScanBtn = new AsyncCommand(StopScan);
        }

        private async void Adapter_ScanTimeoutElapsed(object sender, EventArgs e)
        {
            await StopScan().ConfigureAwait(false);
        }

        private void OnStateChanged(object sender, BluetoothStateChangedArgs e)
        {
            OnPropertyChanged(nameof(BluetoothState));
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
                    if (!string.IsNullOrWhiteSpace(device.Name))
                    {
                        Devices.Add(new DeviceListItemViewModel(device));
                    }
                }
            });
        }


        private async Task StopScan()
        {
            try
            {
                if (Adapter.IsScanning)
                {
                    await Adapter.StopScanningForDevicesAsync();
                    Debug.WriteLine("adapter.StopScanningForDevices()");
                    Dialogs.Toast($"Stopped Scanning For Devices...");
                }
                IsBusy = false;
                IsScanning = false;
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
                if (Device.RuntimePlatform == Device.Android)
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                    if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                    {
                        var permissionResult = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

                        if (permissionResult.First().Value != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                        {
                            Dialogs.Toast("Location Permission denied (please allow). Not scanning.");
                            CrossPermissions.Current.OpenAppSettings();
                            return;
                        }
                    }
                }

                if (App.BTService?.Device?.State == Plugin.BLE.Abstractions.DeviceState.Connected)
                {
                    //We already have device connected so disconnect...
                    await App.BTService.Disconnect();
                }

                if (IsStateOn && !IsRefreshing)
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
            IsScanning = true;
            Devices.Clear();

            foreach (var connectedDevice in Adapter.ConnectedDevices)
            {
                //update rssi for already connected devices (so tha 0 is not shown in the list)
                try
                {
                    await connectedDevice.UpdateRssiAsync();
                }
                catch (Exception ex)
                {
                    Dialogs.Toast($"Failed to update RSSI for {connectedDevice.Name} - {ex.Message}");
                }

                AddOrUpdateDevice(connectedDevice);
            }

            Adapter.ScanMode = ScanMode.LowLatency;
            Adapter.ScanTimeout = 3000;
            await Adapter.StartScanningForDevicesAsync();
            IsScanning = false;
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

        private bool isScanning;
        public bool IsScanning
        {
            get => isScanning;
            private set
            {
                isScanning = value;
                OnPropertyChanged();
            }
        }
        private string stateText;
        public string StateText
        {
            get
            {
                stateText = GetStateText();
                return stateText;
            }
        }
    }
}