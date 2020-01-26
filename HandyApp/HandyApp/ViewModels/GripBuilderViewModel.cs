using Acr.UserDialogs;
using HandyApp.Helpers;
using HandyApp.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace HandyApp.ViewModels
{
    public class GripBuilderViewModel : BaseViewModel
    {
        private IUserDialogs Dialogs;
        public ICommand AddCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public int F0Value { get; set; }
        public int F1Value { get; set; }
        public int F2Value { get; set; }
        public int F34Value { get; set; }
        public int AnimationStepNo { get; set; }
        private int _animationStepperValue;
        public int AnimationStepperValue {
            get => _animationStepperValue;
            set 
            {
                _animationStepperValue = value;
                AnimationStepNo = value;
                OnPropertyChanged(nameof(AnimationStepNo));
            }
        }

        public ObservableCollection<GripPositionCSV> gripPositionCSVs = new ObservableCollection<GripPositionCSV>();

        public GripBuilderViewModel(IUserDialogs dialog)
        {
            Dialogs = dialog;
            AnimationStepNo = 0;
            AddCommand = new Command(ActionAddCommand);
            ClearCommand = new Command(ActionClearCommand);
            SaveCommand = new Command(ActionSaveCommand);
        }

        private void ActionAddCommand()
        {
            try
            {
                GripPositionCSV gripPositionCSV = new GripPositionCSV
                {
                    AnimationStep = AnimationStepNo,
                    CSV = $"{F0Value},{F1Value},{F2Value},{F34Value}"
                };
                gripPositionCSVs.Add(gripPositionCSV);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding the step - {ex.Message}");
            }
        }
        private void ActionClearCommand()
        {
            try
            {
                gripPositionCSVs.Clear();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding the step - {ex.Message}");
            }
        }
        private void ActionSaveCommand()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding the step - {ex.Message}");
            }
        }

    }
}
