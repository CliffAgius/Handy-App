//using Acr.Collections;
using Acr.UserDialogs;
using HandyApp.Models;
using MvvmHelpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HandyApp.ViewModels
{
    public class GripListViewModel : BaseViewModel
    {
        IUserDialogs Dialogs;
        public Command<Grip> DeleteGripCommand { get; private set; }
        public ObservableCollection<Grip> Grips { get; private set; } = new ObservableCollection<Grip>();
        public Command SaveCommand { get; set; }

        public GripListViewModel(IUserDialogs dialogs)
        {
            Dialogs = dialogs;
            GripsCollection gripsCollection = new GripsCollection();
            Grips = gripsCollection.CreateGripsCollection();
            SaveCommand = new Command(ActionSave);
            DeleteGripCommand = new Command<Grip>(ActionDeleteGripCommand);
        }

        private void ActionSave()
        {
            try
            {

                //TODO: Save the new list back to Handy...
                Dialogs.Toast(new ToastConfig("New Grip order saved...") 
                { 
                     Position = ToastPosition.Top,
                     Duration = TimeSpan.FromSeconds(2)
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting the grip - {ex.Message}");
            }
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