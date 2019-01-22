using Plugin.BluetoothLE;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceConnectionView : ContentPage
    {
        public DeviceConnectionView(IDevice device)
        {
            InitializeComponent();
            BindingContext = new ViewModels.DeviceConnectionViewModel(device);
        }
    }
}