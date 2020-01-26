using HandyApp.Helpers;
using HandyApp.Models;
using MvvmHelpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HandyApp.ViewModels
{
    public class GripBuilderViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public int AnimationStepNo { get; set; }

        public int F0Value { get; set; }
        public int F1Value { get; set; }
        public int F2Value { get; set; }
        public int F34Value { get; set; }

        public GripBuilderViewModel()
        {
            AnimationStepNo = 0;
        }

    }
}
