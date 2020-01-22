using Acr.UserDialogs;
using HandyApp.Helpers;
using HandyApp.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HandyApp.ViewModels
{
    public class HandControlViewModel : BaseViewModel
    {
        IUserDialogs Dialogs;
        public ICommand FavoriteCommand { get; private set; }
        public ICommand TappedCommand { get; private set;  }
        public ObservableCollection<Grip> Grips { get; private set; }

        public HandControlViewModel(IUserDialogs dialogs)
        {
            Dialogs = dialogs;
            if (App.BTService.Device is null)
            {
                Dialogs.Alert("Not Connected please connect first...");
                //We have no BT Connection yet so goto the the Connection page first...
                AppShell.Current.GoToAsync("BTConnection");
            }
            GripsCollection gripsCollection = new GripsCollection();
            Grips = gripsCollection.CreateGripsCollection();

            FavoriteCommand = new Command<Grip>(ActionFavoriteChangeCommand);
            TappedCommand = new AsyncCommand<string>(ActionTappedCommand);
        }

        private async Task ActionTappedCommand(string gripUARTCmmand)
        {
            Dialogs.Toast($"Sending the command to HANDY - {gripUARTCmmand}");
            await App.BTService.SendUARTCommand(gripUARTCmmand).ConfigureAwait(false);
        }

        private void ActionFavoriteChangeCommand(Grip arg)
        {
            try
            {
                arg.IsFavorite = !arg.IsFavorite;
                if (arg.IsFavorite)
                {
                    arg.FavoriteIcon = IconFont.Heart;
                }
                else
                {
                    arg.FavoriteIcon = IconFont.HeartOutline;
                }
                Grips[Grips.FirstOrDefault(x => x.GripNumber == arg.GripNumber).GripNumber - 1] = arg;
                OnPropertyChanged(nameof(Grips));
            }
            catch (Exception ex)
            {
                Dialogs.Toast($"Sorry there has been a fault - {ex.Message}");
            }
        }
    }
}
