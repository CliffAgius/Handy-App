using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceConnectionView : ContentPage
    {
        public DeviceConnectionView()
        {
            InitializeComponent();
            BindingContext = new ViewModels.DeviceConnectionViewModel(UserDialogs.Instance);
        }
    }
}