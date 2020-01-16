
using MvvmHelpers;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;

namespace HandyApp.ViewModels
{
    public class DeviceListItemViewModel : BaseViewModel
    {
        public IDevice Device { get; private set; }

        public Guid Id => Device.Id;
        public bool IsConnected => Device.State == DeviceState.Connected;
        public int Rssi => Device.Rssi;
        public string Name => Device.Name;

        public DeviceListItemViewModel(IDevice device)
        {
            Device = device;
        }

        public void Update(IDevice newDevice = null)
        {
            if (newDevice != null)
            {
                Device = newDevice;
            }
            OnPropertyChanged(nameof(IsConnected));
            OnPropertyChanged(nameof(Rssi));
        }
    }
}