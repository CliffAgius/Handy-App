using Acr.UserDialogs;
using HandyApp.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HandyApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private IUserDialogs Dialogs;
        public ICommand SaveCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand GetSystemDiagCommand { get; private set; }

        public string HoldTime { get; set; }
        public SelectedHand SelectedHand { get; set; }
        public string PeakThreshold { get; set; }
        public bool MotorsEnabled { get; set; }
        public bool DemoModeEnabled { get; set; }
        public MuscleMode MuscleMode { get; set; }

        public List<string> HandNames
        {
            get
            {
                return Enum.GetNames(typeof(SelectedHand)).ToList();
            }
        }

        public List<string> MuscleModes
        {
            get
            {
                return Enum.GetNames(typeof(MuscleMode)).ToList();
            }
        }

        public SettingsViewModel(IUserDialogs dialog)
        {
            Dialogs = dialog;
            SaveCommand = new Command(ActionSaveCommand);
            ResetCommand = new Command(ActionResetCommand);
            GetSystemDiagCommand = new AsyncCommand(ActionGetSystemDiagCommand);
            SetValues();

            //Listen to the UART replies...
            App.BTService.RcvdDataHandler += BTService_RcvdDataHandler;
        }

        private void BTService_RcvdDataHandler(object sender, EventArgs e)
        {
            try
            {
                string UARTString = App.BTService.RcvdDataString;
                Dialogs.Alert(new AlertConfig
                {
                    Title = "UART Data recieved",
                    Message = UARTString,
                    OkText = "OK"
                });
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an error while recieving the command - {ex.Message}");
            }
        }

        private void SetValues()
        {
            HoldTime = "600";
            SelectedHand = SelectedHand.Left;
            PeakThreshold = "300";
            MotorsEnabled = true;
            DemoModeEnabled = false;
            MuscleMode = MuscleMode.Simple;
        }

        private async Task ActionGetSystemDiagCommand()
        {
            try
            {
                await App.BTService.SendUARTCommand("#");
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an Error has occured - {ex.Message}");
            }
        }

        private void ActionResetCommand()
        {
            try
            {
                SetValues();
                OnPropertyChanged(string.Empty);
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an Error has occured - {ex.Message}");
            }
        }

        private void ActionSaveCommand()
        {
            try
            {
                Dialogs.Confirm(new ConfirmConfig
                {
                    CancelText = "Cancel",
                    OkText = "Accept",
                    OnAction = CheckSaveAction,
                    Message = "Are you sure you wish to save these settings...",
                    Title = "Save Changes"
                });
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an Error has occured - {ex.Message}");
            }
        }

        private async void CheckSaveAction(bool actionSave)
        {
            try
            {
                if (actionSave)
                {
                    await App.BTService.SendUARTCommand($"T{HoldTime}");
                    await App.BTService.SendUARTCommand($"U{PeakThreshold}");
                    await App.BTService.SendUARTCommand($"H{SelectedHand + 1}");
                    await App.BTService.SendUARTCommand($"M{MuscleMode + 1}");


                }
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an Error has occured - {ex.Message}");
            }
        }
    }
}
