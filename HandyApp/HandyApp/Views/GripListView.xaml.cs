using Acr.UserDialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GripListView: ContentPage
    {
        public GripListView()
        {
            InitializeComponent();
            BindingContext = new ViewModels.GripListViewModel(UserDialogs.Instance);
        }
    }
}
