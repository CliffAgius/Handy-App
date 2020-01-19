using HandyApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GripBuilderView : ContentPage
    {
        public GripBuilderView()
        {
            InitializeComponent();

            BindingContext = new GripBuilderViewModel();
        }
    }
}