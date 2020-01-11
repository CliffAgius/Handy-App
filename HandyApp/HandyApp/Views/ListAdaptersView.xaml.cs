using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListAdaptersView : ContentPage
    {
        public ListAdaptersView()
        {
            InitializeComponent();
            BindingContext = new ViewModels.ListAdaptersViewModel(UserDialogs.Instance);
        }
    }
}