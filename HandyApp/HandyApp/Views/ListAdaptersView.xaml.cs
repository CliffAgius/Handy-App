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
            //Asign the Navigation root so that the Nav system knows wher to start from...
            App.NavigationRoot = this;
            BindingContext = new ViewModels.ListAdaptersViewModel(UserDialogs.Instance);
        }
    }
}