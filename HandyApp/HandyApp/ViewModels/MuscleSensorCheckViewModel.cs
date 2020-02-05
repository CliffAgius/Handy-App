using Acr.UserDialogs;
using HandyApp.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HandyApp.ViewModels
{
    public class MuscleSensorCheckViewModel : BaseViewModel
    {
        private IUserDialogs Dialogs;
        private int openSensorValue;
        private int closeSensorValue;
        private List<SensorData> SensorValues = new List<SensorData>();

        public AsyncCommand StartMeasurementCommand { get; private set; }
        public AsyncCommand ProcessCommand { get; set; }

        public int OpenSensorValue
        {
            get => openSensorValue;
            set
            {
                openSensorValue = value;
                OnPropertyChanged();
            }
        }
        public int CloseSensorValue
        {
            get => closeSensorValue;
            set
            {
                closeSensorValue = value;
                OnPropertyChanged();
            }
        }

        public bool ProcessBtnEnabled { get; set; }

        public MuscleSensorCheckViewModel(IUserDialogs dialog)
        {
            Dialogs = dialog;
            StartMeasurementCommand = new AsyncCommand(ActionStartMeasurement);
            ProcessCommand = new AsyncCommand(ActionProcessing);
            IsBusy = true;
            ProcessBtnEnabled = true;
            OpenSensorValue = 0;
            CloseSensorValue = 0;

            ////REMOVE ME....
            //AddDummySensorData();
            ////***************

            //Listen to the UART replies...
            App.BTService.RcvdDataHandler += BTService_RcvdDataHandler;
        }

        //private void AddDummySensorData()
        //{
        //    SensorValues.Add(new SensorData { OpenSensorReading = 10, CloseSensorReading = 20 });
        //    SensorValues.Add(new SensorData { OpenSensorReading = 20, CloseSensorReading = 30 });
        //    SensorValues.Add(new SensorData { OpenSensorReading = 13, CloseSensorReading = 40 });
        //    SensorValues.Add(new SensorData { OpenSensorReading = 45, CloseSensorReading = 50 });
        //    SensorValues.Add(new SensorData { OpenSensorReading = 464, CloseSensorReading = 60 });
        //    SensorValues.Add(new SensorData { OpenSensorReading = 456, CloseSensorReading = 70 });
        //    SensorValues.Add(new SensorData { OpenSensorReading = 1076, CloseSensorReading = 80 });
        //    SensorValues.Add(new SensorData { OpenSensorReading = 1045, CloseSensorReading = 90 });

        //}

        private async Task ActionStartMeasurement()
        {
            IsBusy = false;
            await App.BTService.SendUARTCommand("A7").ConfigureAwait(false);
            OpenSensorValue = 50;
            CloseSensorValue = 50;
        }


        //In here we will send from mobile to the Azure Function for processing and storage...
        private async Task ActionProcessing()
        {
            try
            {
                var sensorJSON = JsonConvert.SerializeObject(SensorValues);

                var url = $"https://handyfunction.azurewebsites.net/api/Function1?" +
                            $"sensorJSON={sensorJSON}";

                using (var client = new HttpClient())
                {
                    var result = await client.GetStringAsync(url);
                }
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error sneding the data to the cloud - {ex.Message}");

            }
        }

        private void BTService_RcvdDataHandler(object sender, EventArgs e)
        {
            try
            {
                string UARTString = App.BTService.RcvdDataString;
                if (!UARTString.Contains("*"))
                {
                    //SensorValues.Add(UARTString);
                }
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error while recieving the command - {ex.Message}");
            }
        }
    }
}
