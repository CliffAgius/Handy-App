//using Acr.Collections;
using HandyApp.Models;
using System;
using System.Collections.ObjectModel;

namespace HandyApp.ViewModels
{
    public class GripListViewModel : ViewModel
    {

        public ObservableCollection<Grips> GripsCollection { get; private set; } = new ObservableCollection<Grips>();

        public GripListViewModel()
        {
            BuildGripsCollection();
        }

        void BuildGripsCollection()
        {
            GripsCollection.Add(new Grips { ID = 0, GripNumber = 1, GripName = "Fist Grip" });
            GripsCollection.Add(new Grips { ID = 1, GripNumber = 2, GripName = "Palm Grip" });
            GripsCollection.Add(new Grips { ID = 2, GripNumber = 3, GripName = "Thumbs Up" });
            GripsCollection.Add(new Grips { ID = 3, GripNumber = 4, GripName = "Point" });
            GripsCollection.Add(new Grips { ID = 4, GripNumber = 5, GripName = "Pinch / OK" });
            GripsCollection.Add(new Grips { ID = 5, GripNumber = 6, GripName = "Tripod" });
        }
    }
}