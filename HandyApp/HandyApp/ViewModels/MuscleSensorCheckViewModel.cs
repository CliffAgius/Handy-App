using Acr.UserDialogs;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandyApp.ViewModels
{
    public class MuscleSensorCheckViewModel : BaseViewModel
    {
        private IUserDialogs Dialogs;
        private int openSensorValue;
        private int closeSensorValue;
        private List<string> SensorValues = new List<string>();

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
            ProcessBtnEnabled = false;
            OpenSensorValue = 0;
            CloseSensorValue = 0;
            //Listen to the UART replies...
            App.BTService.RcvdDataHandler += BTService_RcvdDataHandler;
        }

        private async Task ActionStartMeasurement()
        {
            IsBusy = false;
            await App.BTService.SendUARTCommand("A7").ConfigureAwait(false);
            OpenSensorValue = 50;
            CloseSensorValue = 50;
        }

        private Task ActionProcessing()
        {
            //In here we will send from mobile to the Azure Function for processing and storage...
            throw new NotImplementedException();
        }

        private void BTService_RcvdDataHandler(object sender, EventArgs e)
        {
            try
            {
                string UARTString = App.BTService.RcvdDataString;
                if (!UARTString.Contains("*"))
                {
                    SensorValues.Add(UARTString);
                }
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error while recieving the command - {ex.Message}");
            }
        }
    }
}
