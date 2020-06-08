using System;

namespace HandyApp.Models
{
    public class SensorData
    {
        public int OpenSensorReading { get; set; }
        public int CloseSensorReading { get; set; }

        public bool IsDirty { get; set; }

        public DateTime SensorReadDate{ get; set; }

        public string UserID { get; set; }

    }
}
