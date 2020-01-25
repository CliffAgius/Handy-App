//using Acr.Collections;
using HandyApp.Models;
using MvvmHelpers;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace HandyApp.ViewModels
{
    public class GripListViewModel : BaseViewModel
    {

        public ObservableCollection<Grip> Grips { get; private set; } = new ObservableCollection<Grip>();

        public GripListViewModel()
        {
            GripsCollection gripsCollection = new GripsCollection();
            Grips = gripsCollection.CreateGripsCollection();
        }
    }
}