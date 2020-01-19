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

        public ObservableCollection<Grip> GripsCollection { get; private set; } = new ObservableCollection<Grip>();

        public GripListViewModel()
        {
            BuildGripsCollection();
        }

        void BuildGripsCollection()
        {
            GripsCollection.Add(new Grip { ID = 0, GripNumber = 1, GripName = "Fist Grip" });
            GripsCollection.Add(new Grip { ID = 1, GripNumber = 2, GripName = "Palm Grip" });
            GripsCollection.Add(new Grip { ID = 2, GripNumber = 3, GripName = "Thumbs Up" });
            GripsCollection.Add(new Grip { ID = 3, GripNumber = 4, GripName = "Point" });
            GripsCollection.Add(new Grip { ID = 4, GripNumber = 5, GripName = "Pinch / OK" });
            GripsCollection.Add(new Grip { ID = 5, GripNumber = 6, GripName = "Tripod" });
        }
    }
}