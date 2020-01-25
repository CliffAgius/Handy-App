using Acr.UserDialogs;
using HandyApp.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using static HandyApp.Helpers.ObjectExtensions;

namespace HandyApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private IUserDialogs Dialogs;
        public ICommand SaveCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand GetSystemDiagCommand { get; private set; }

        public HandSettings CurrentHandSettings { get; set; }
        public HandSettings UserHandSettings { get; set; }

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
            CurrentHandSettings = new HandSettings
            {
                HoldTime = "600",
                CurrentHand = SelectedHand.Left,
                PeakThreshold = "300",
                MotorsEnabled = true,
                DemoModeEnabled = false,
                MuscleMode = MuscleMode.Simple
            };

            UserHandSettings = new HandSettings
            {
                HoldTime = "600",
                CurrentHand = SelectedHand.Left,
                PeakThreshold = "300",
                MotorsEnabled = true,
                DemoModeEnabled = false,
                MuscleMode = MuscleMode.Simple
            };
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
                    List<Variance> variances = CurrentHandSettings.DetailedCompare(UserHandSettings);

                    foreach (var item in variances)
                    {
                        switch (item.Property)
                        {
                            case "HoldTime":
                                await App.BTService.SendUARTCommand($"T{item.NewValue}");
                                break;
                            case "CurrentHand":
                                await App.BTService.SendUARTCommand($"H{(int)item.NewValue}");
                                break;
                            case "PeakThreshold":
                                await App.BTService.SendUARTCommand($"U{item.NewValue}");
                                break;
                            case "MotorsEnabled":
                                await App.BTService.SendUARTCommand("A3");
                                break;
                            case "DemoModeEnabled":
                                await App.BTService.SendUARTCommand("A0");
                                break;
                            case "MuscleMode":
                                await App.BTService.SendUARTCommand($"M{(int)item.NewValue}");
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry an Error has occured - {ex.Message}");
            }
        }
    }
}
