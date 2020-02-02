using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BTConnectionView : ContentPage
    {
        public BTConnectionView()
        {
            InitializeComponent();

            BindingContext = new ViewModels.BTConnectionViewModel(UserDialogs.Instance);
        }
    }
}