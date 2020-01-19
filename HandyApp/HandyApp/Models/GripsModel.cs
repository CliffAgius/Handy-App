using Xamarin.Forms;

namespace HandyApp.Models
{
    public class Grip
    {
        public int ID { get; set; }
        public int GripNumber { get; set; }
        public string GripName { get; set; }
        public string GripCommand { get; set; }
        public ImageSource GripImageSrc { get; set; }
        public bool IsFavorite { get; set; }
        public string FavoriteIcon { get; set; }
        public string GripColor { get; set; }
    }
}
