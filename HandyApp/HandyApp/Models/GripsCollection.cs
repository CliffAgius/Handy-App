using HandyApp.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace HandyApp.Models
{
    public class GripsCollection
    {
        public ObservableCollection<Grip> CreateGripsCollection()
        {
            List<Grip> source = new List<Grip>();
            source.Add(new Grip
            {
                GripNumber = 1,
                GripName = "Fist",
                GripCommand = "G0",
                GripColor = "#99FF0000",
                FavoriteIcon = IconFont.HeartOutline,
                GripImageSrc = ImageSource.FromResource("HandyApp.Images.okhand.png"),
                IsFavorite = false
            });
            source.Add(new Grip
            {
                GripNumber = 2,
                GripName = "Hook",
                GripCommand = "G1",
                GripColor = "#9900FF00",
                FavoriteIcon = IconFont.HeartOutline,
                GripImageSrc = ImageSource.FromResource("HandyApp.Images.okhand.png"),
                IsFavorite = false
            });
            source.Add(new Grip
            {
                GripNumber = 3,
                GripName = "Point",
                GripCommand = "G2",
                GripColor = "#990000FF",
                FavoriteIcon = IconFont.Heart,
                GripImageSrc = ImageSource.FromResource("HandyApp.Images.okhand.png"),
                IsFavorite = false
            });
            source.Add(new Grip
            {
                GripNumber = 4,
                GripName = "Pinch",
                GripCommand = "G3",
                GripColor = "#998E8E93",
                FavoriteIcon = IconFont.HeartOutline,
                GripImageSrc = ImageSource.FromResource("HandyApp.Images.okhand.png"),
                IsFavorite = false
            });
            source.Add(new Grip
            {
                GripNumber = 5,
                GripName = "Tripod",
                GripCommand = "G4",
                GripColor = "#998E8E93",
                FavoriteIcon = IconFont.HeartOutline,
                GripImageSrc = ImageSource.FromResource("HandyApp.Images.okhand.png"),
                IsFavorite = false
            });
            source.Add(new Grip
            {
                GripNumber = 6,
                GripName = "Finger Roll",
                GripCommand = "G5",
                GripColor = "#998E8E93",
                FavoriteIcon = IconFont.HeartOutline,
                GripImageSrc = ImageSource.FromResource("HandyApp.Images.okhand.png"),
                IsFavorite = false
            });

            return new ObservableCollection<Grip>(source);
        }
    }
}
