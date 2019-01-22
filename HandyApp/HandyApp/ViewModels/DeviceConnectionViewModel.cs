using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandyApp.ViewModels
{
    public class DeviceConnectionViewModel
    {
        IDevice device;

        public DeviceConnectionViewModel(IDevice selecteddevice)
        {
            var ddevice = selecteddevice;
        }

    }
}
