//using Acr.Collections;
using HandyApp.Models;
using MvvmHelpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace HandyApp.ViewModels
{
    public class GripListViewModel : BaseViewModel
    {
        public Command<Grip> DeleteGripCommand { get; private set; }
        public ObservableCollection<Grip> Grips { get; private set; } = new ObservableCollection<Grip>();

        public GripListViewModel()
        {
            GripsCollection gripsCollection = new GripsCollection();
            Grips = gripsCollection.CreateGripsCollection();

            DeleteGripCommand = new Command<Grip>(ActionDeleteGripCommand);
        }

        private void ActionDeleteGripCommand(Grip obj)
        {
            try
            {
                Grips.Remove(obj);
                OnPropertyChanged();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting the grip - {ex.Message}");
            }
        }
    }
}