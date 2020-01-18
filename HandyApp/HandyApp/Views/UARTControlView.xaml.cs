using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UARTControlView : ContentPage
    {
        public UARTControlView()
        {
            InitializeComponent();
            BindingContext = new ViewModels.UARTControlViewModel(UserDialogs.Instance);
        }
    }
}