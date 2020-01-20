using HandyApp.Helpers;
using HandyApp.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HandyApp.ViewModels
{
    public class GripBuilderViewModel : BaseViewModel
    {
        public readonly IList<Grip> source;
        public ICommand FavoriteCommand { get; private set; }

        public ObservableCollection<Grip> Grips { get; private set; }

        public GripBuilderViewModel()
        {
            source = new List<Grip>();
            CreateGripsCollection();

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

        private void CreateGripsCollection()
        {
            source.Add(new Grip { 
                GripNumber = 1, 
                GripName = "Fist", 
                GripCommand = "G0",
                GripColor = "Red",
                FavoriteIcon = IconFont.HeartOutline,
                GripImageSrc = ImageSource.FromResource("HandyApp.Images.okhand.png"), 
                IsFavorite= false 
            });
            source.Add(new Grip
            {
                GripNumber = 2,
                GripName = "Hook",
                GripCommand = "G1",
                GripColor = "Blue",
                FavoriteIcon = IconFont.HeartOutline,
                GripImageSrc = ImageSource.FromResource("HandyApp.Images.okhand.png"),
                IsFavorite = false
            });
            source.Add(new Grip
            {
                GripNumber = 3,
                GripName = "Point",
                GripCommand = "G2",
                GripColor = "Green",
                FavoriteIcon = IconFont.Heart,
                GripImageSrc = ImageSource.FromResource("HandyApp.Images.okhand.png"),
                IsFavorite = false
            });
            source.Add(new Grip
            {
                GripNumber = 4,
                GripName = "Pinch",
                GripCommand = "G3",
                GripColor = "#8E8E93",
                FavoriteIcon = IconFont.HeartOutline,
                GripImageSrc = ImageSource.FromResource("HandyApp.Images.okhand.png"),
                IsFavorite = false
            });
            source.Add(new Grip
            {
                GripNumber = 5,
                GripName = "Tripod",
                GripCommand = "G4",
                GripColor = "#8E8E93",
                FavoriteIcon = IconFont.HeartOutline,
                GripImageSrc = ImageSource.FromResource("HandyApp.Images.okhand.png"),
                IsFavorite = false
            });
            source.Add(new Grip
            {
                GripNumber = 6,
                GripName = "Finger Roll",
                GripCommand = "G5",
                GripColor = "#8E8E93",
                FavoriteIcon = IconFont.HeartOutline,
                GripImageSrc = ImageSource.FromResource("HandyApp.Images.okhand.png"),
                IsFavorite = false
            });

            Grips = new ObservableCollection<Grip>(source);
        }
    }
}
