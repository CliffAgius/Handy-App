using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HandyApp.Views
{
    public partial class HomeView : ContentPage
    {
        public HomeView()
        {
            InitializeComponent();
            //Asign the Navigation root so that the Nav system knows wher to start from...
            App.NavigationRoot = this;
        }
    }
}
