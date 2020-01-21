using HandyApp.Helpers;
using HandyApp.Models;
using MvvmHelpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HandyApp.ViewModels
{
    public class GripBuilderViewModel : BaseViewModel
    {
        public ICommand FavoriteCommand { get; private set; }

        public ObservableCollection<Grip> Grips { get; private set; }

        public GripBuilderViewModel()
        {
            GripsCollection gripsCollection = new GripsCollection();
            Grips = gripsCollection.CreateGripsCollection();

            FavoriteCommand = new MvvmHelpers.Commands.Command<Grip>(ActionFavoriteChangeCommand);
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
                
            }
        }
    }
}
