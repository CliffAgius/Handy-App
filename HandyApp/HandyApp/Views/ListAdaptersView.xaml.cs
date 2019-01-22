using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListAdaptersView : ContentPage
    {
        public ListAdaptersView()
        {
            InitializeComponent();

            BindingContext = new ViewModels.ListAdaptersViewModel();
        }
    }
}