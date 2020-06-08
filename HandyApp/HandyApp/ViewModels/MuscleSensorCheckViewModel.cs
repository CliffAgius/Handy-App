using Acr.UserDialogs;
using HandyApp.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HandyApp.ViewModels
{
    public class MuscleSensorCheckViewModel : BaseViewModel
    {
        private IUserDialogs Dialogs;
        private int openSensorValue;
        private int closeSensorValue;
        private int SensorReadCount = 0;
        private List<SensorData> SensorValues = new List<SensorData>();

        public AsyncCommand StartMeasurementCommand { get; private set; }
        public AsyncCommand ProcessCommand { get; set; }

        public string UARTRcvd { get; set; }

        public int OpenSensorValue
        {
            get
            {
                return openSensorValue;
            }

            set
            {
                openSensorValue = value;
                OnPropertyChanged();
            }
        }
        public int CloseSensorValue
        {
            get
            {
                return closeSensorValue;
            }

            set
            {
                closeSensorValue = value;
                OnPropertyChanged();
            }
        }

        public bool ProcessBtnEnabled { get; set; }

        public MuscleSensorCheckViewModel(IUserDialogs dialog)
        {
            try
            {
                Dialogs = dialog;
                StartMeasurementCommand = new AsyncCommand(ActionStartMeasurement);
                ProcessCommand = new AsyncCommand(ActionProcessing);
                IsBusy = true;
                ProcessBtnEnabled = true;
                OpenSensorValue = 0;
                CloseSensorValue = 0;

                //Listen to the UART replies...
                App.BTService.RcvdDataHandler += BTService_RcvdDataHandler;

            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error during page set-up - {ex.Message}");
            }
        }

        private async Task ActionStartMeasurement()
        {
            try
            {
                IsBusy = false;
                SensorReadCount = 0;
                await App.BTService.SendUARTCommand("M0");
                await App.BTService.SendUARTCommand("A7");
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error sneding the command - {ex.Message}");
            }
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
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    UARTRcvd += App.BTService.RcvdDataString;
                    if (UARTRcvd.Contains("A7"))
                    {
                        UARTRcvd = "";
                    }
                    if (UARTRcvd.EndsWith("\r\n") && UARTRcvd.Length > 4)
                    {
                        string[] command = UARTRcvd.Split('-');
                        if (command.Length is 2)
                        {
                            SensorValues.Add(new SensorData
                            {
                                OpenSensorReading = int.Parse(command[0].Trim()),
                                CloseSensorReading = int.Parse(command[1].Trim()),
                                SensorReadDate = DateTime.UtcNow,
                                UserID = DeviceInfo.Name
                            });

                            SensorReadCount++;
                            UARTRcvd = "";

                            if (SensorReadCount >= 20)
                            {
                                OpenSensorValue = 0;
                                CloseSensorValue = 0;
                                IsBusy = true;
                                _ = EndOfSensorReadings();
                            }
                            else
                            {
                                OpenSensorValue = int.Parse(command[0].Trim());
                                CloseSensorValue = int.Parse(command[1].Trim());
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error while recieving the command - {ex.Message}");
            }
        }

        private async Task EndOfSensorReadings()
        {
            OpenSensorValue = SensorValues.Max(o => o.OpenSensorReading);
            CloseSensorValue = SensorValues.Max(c => c.CloseSensorReading);
            await App.BTService.SendUARTCommand("M2");
        }
    }
}
