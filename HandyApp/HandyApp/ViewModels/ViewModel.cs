using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Text;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace HandyApp.ViewModels
{
    public abstract class ViewModel : ReactiveObject, INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        //public void RaisePropertyChanged(params string[] propertyNames)
        //{
        //    foreach (var propertyName in propertyNames)
        //    {
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        public bool IsBusy { get; set; }

        protected CompositeDisposable DestroyWith { get; } = new CompositeDisposable();
        CompositeDisposable deactivateWith;
        protected CompositeDisposable DeactivateWith
        {
            get
            {
                if (this.deactivateWith == null)
                    this.deactivateWith = new CompositeDisposable();

                return this.deactivateWith;
            }
        }

        public virtual void OnDisappearing()
        {
            this.deactivateWith?.Dispose();
            this.deactivateWith = null;
        }


        public virtual void Destroy()
        {
            this.DestroyWith?.Dispose();
        }
    }
}
