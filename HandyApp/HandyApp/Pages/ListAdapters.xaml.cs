using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandyApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListAdapters : ContentPage
    {
        public ListAdapters()
        {
            InitializeComponent();

            var vm = new ViewModels.ListAdaptersViewModel();

            BindingContext = vm;
        }
    }
}