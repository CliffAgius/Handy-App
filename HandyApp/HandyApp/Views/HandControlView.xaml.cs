using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HandControlView : ContentPage
    {
        public HandControlView()
        {
            InitializeComponent();

            BindingContext = new ViewModels.HandControlViewModel(UserDialogs.Instance);
        }
    }
}