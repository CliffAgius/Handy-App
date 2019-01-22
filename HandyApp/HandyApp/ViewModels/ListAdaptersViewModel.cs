using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;
using System.Reactive.Linq;
using ReactiveUI;
using Acr.Collections;
using System.Linq;
using System.Reactive.Disposables;

namespace HandyApp.ViewModels
{
    public class ListAdaptersViewModel : ViewModel
    {
        public ObservableList<ScanResultViewModel> Devices { get; } = new ObservableList<ScanResultViewModel>();
        public ObservableCollection<IAdapter> Adapters { get; } = new ObservableCollection<IAdapter>();
        private readonly IUserDialogs dialogs;
        IAdapterScanner adapterScanner;
        IAdapter adapter;
        IDisposable scan;

        public ICommand ScanCommand { get; }
        public ICommand ItemTappedCommand { get; private set; }
        public ICommand OpenSettings { get; }

        public string SelectedDevice { get; set; }
        public string AdapterName { get; set; }

        public ListAdaptersViewModel()
        {
            adapterScanner = CrossBleAdapter.AdapterScanner;
            ItemTappedCommand = ReactiveCommand.Create<ScanResultViewModel>(x => {
                var test = "d";
            });
            GetAdapter();
            if (adapter.Status != AdapterStatus.PoweredOn)
            {
                ToggleAdapter();
            }

            AdapterName = adapter.Status.ToString();

            CrossBleAdapter.Current.WhenStatusChanged().Subscribe(status => { IsBusy = adapter.IsScanning; });

            this.ScanCommand = ReactiveCommand.Create(() =>
            {
                if (IsScanning)
                {
                    scan?.Dispose();
                    IsScanning = false;
                }
                else
                {
                    Devices.Clear();

                    IsScanning = true;
                    scan = adapter
                        .Scan()
                        .Buffer(TimeSpan.FromSeconds(1))
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(
                            results =>
                            {
                                var list = new List<ScanResultViewModel>();
                                foreach (var result in results)
                                {
                                    var dev = Devices.FirstOrDefault(x => x.Uuid.Equals(result.Device.Uuid));

                                    if (dev != null)
                                    {
                                        dev.TrySet(result);
                                    }
                                    else
                                    {
                                        dev = new ScanResultViewModel();
                                        dev.TrySet(result);
                                        if (dev.Name != null)
                                        {
                                            //Check there is a name to display otherwise no point adding to the list...
                                            list.Add(dev);
                                        }
                                    }
                                }
                                if (list.Any())
                                    Devices.AddRange(list);
                            },
                            ex => dialogs.Alert(ex.ToString(), "ERROR")
                        )
                        .DisposeWith(DeactivateWith);
                }
            });

            this.OpenSettings = ReactiveCommand.Create(() =>
            {
                if (this.adapter.Features.HasFlag(AdapterFeatures.OpenSettings))
                {
                    this.adapter.OpenSettings();
                }
                else
                {
                    dialogs.Alert("Cannot open bluetooth settings");
                }
            });

        }

        private void HandleItemTapped(ScanResultViewModel obj)
        {
            throw new NotImplementedException();
        }

        void GetAdapter()
        {
            adapterScanner
                   .FindAdapters()
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(
                       this.Adapters.Add,
                       ex => dialogs.Alert(ex.ToString(), "Error"),
                       async () =>
                       {
                           this.IsBusy = false;
                           switch (this.Adapters.Count)
                           {
                               case 0:
                                   dialogs.Alert("No BluetoothLE Adapters Found");
                                   break;

                               case 1:
                                   adapter = this.Adapters.First();
                                   //await navigationService.NavToAdapter(adapter);
                                   break;
                           }
                       }
                   );
        }

        void ToggleAdapter()
        {
            if (this.adapter.CanControlAdapterState())
            {
                var poweredOn = this.adapter.Status == AdapterStatus.PoweredOn;
                this.adapter.SetAdapterState(!poweredOn);
            }
            else
            {
                dialogs.Alert("Cannot change bluetooth adapter state");
            }
        }

        public string Title { get; private set; }
        public bool IsScanning { get; private set; }

    }
}