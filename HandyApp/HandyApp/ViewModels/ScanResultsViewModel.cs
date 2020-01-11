//using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HandyApp.ViewModels
{
    public class ScanResultViewModel
    {
        //public IDevice Device { get; set; }

        public string Name { get; set; }
        public bool IsConnected { get;set; }
        public Guid Uuid { get; set; }
        public int Rssi { get; set; }
        public bool IsConnectable { get; set; }
        public int ServiceCount { get; set; }
        public string ManufacturerData { get; set; }
        public string LocalName { get; set; }
        public int TxPower { get; set; }


        //public bool TrySet(IScanResult result)
        //{
        //    var response = false;

        //    if (this.Uuid == Guid.Empty)
        //    {
        //        this.Device = result.Device;
        //        this.Uuid = this.Device.Uuid;

        //        response = true;
        //    }

        //    try
        //    {
        //        if (this.Uuid == result.Device.Uuid)
        //        {
        //            response = true;

        //            this.Name = result.Device.Name;
        //            this.Rssi = result.Rssi;

        //            var ad = result.AdvertisementData;
        //            this.ServiceCount = ad.ServiceUuids?.Length ?? 0;
        //            this.IsConnectable = ad.IsConnectable;
        //            this.LocalName = ad.LocalName;
        //            this.TxPower = ad.TxPower;
        //            this.ManufacturerData = ad.ManufacturerData == null
        //                ? null
        //                : BitConverter.ToString(ad.ManufacturerData);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //    }
        //    return response;
        //}
    }
}
