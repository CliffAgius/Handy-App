using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Acr.UserDialogs;
using System.Reactive.Linq;
using ReactiveUI;
using Acr.Collections;
using System.Linq;
using System.Reactive.Disposables;
using HandyApp.Views;
using Xamarin.Forms;

namespace HandyApp.ViewModels
{
    public class ListAdaptersViewModel : ViewModel
    {
        public ObservableList<ScanResultViewModel> Devices { get; } = new ObservableList<ScanResultViewModel>();
        public ObservableCollection<IAdapter> Adapters { get; } = new ObservableCollection<IAdapter>();
        readonly IAdapterScanner adapterScanner;
        IAdapter adapter;
        IDisposable scan;
        IUserDialogs Dialogs;

        public ICommand ScanCommand { get; }
        public ICommand ItemTappedCommand { get; private set; }
        public ICommand OpenSettings { get; }

        public string SelectedDevice { get; set; }
        public string AdapterName { get; set; }

        public ListAdaptersViewModel(IUserDialogs dialogs)
        {
            try
            {
                IsScanning = false;
                Dialogs = dialogs;
                adapterScanner = CrossBleAdapter.AdapterScanner;
                GetAdapter();
                if (adapter.Status != AdapterStatus.PoweredOn)
                {
                    ToggleAdapter();
                }

                AdapterName = adapter.Status.ToString();

                CrossBleAdapter.Current.WhenStatusChanged().Subscribe(status => { IsBusy = adapter.IsScanning; });

                ScanCommand = ReactiveCommand.Create(() =>
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
                                ex => Dialogs.Alert(ex.ToString(), "ERROR")
                            )
                            .DisposeWith(DeactivateWith);
                    }
                });

                OpenSettings = ReactiveCommand.Create(() =>
                {
                    if (adapter.Features.HasFlag(AdapterFeatures.OpenSettings))
                    {
                        adapter.OpenSettings();
                    }
                    else
                    {
                        Dialogs.Alert("Cannot open bluetooth settings");
                    }
                });

                ItemTappedCommand = ReactiveCommand.Create<ScanResultViewModel>(async SelectedDevice =>
                {
                    try
                    {
                        //Save the selected device to the Global Device holder...
                        App.device = SelectedDevice.Device;
                        //Dispose of the Scanner so that it stop scanning...
                        scan.Dispose();
                        //Navigate to the Device View...
                        await App.NavigateToAsync(new DeviceConnectionView()).ConfigureAwait(false);

                    }
                    catch (Exception ex)
                    {
                        Dialogs.Alert($"Sorry the navigation has failed. {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                Dialogs.Alert($"Sorry there is a fault - {ex.Message}");
            }

        }

        void GetAdapter()
        {
            adapterScanner
                   .FindAdapters()
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(
                       Adapters.Add,
                       ex => Dialogs.Alert(ex.ToString(), "Error"),
                       () =>
                       {
                           IsBusy = false;
                           if (Adapters.Count == 0) Dialogs.Alert("No BluetoothLE Adapters Found");
                           else adapter = Adapters.First();
                       }
                   );
        }

        void ToggleAdapter()
        {
            if (adapter.CanControlAdapterState())
            {
                var poweredOn = adapter.Status == AdapterStatus.PoweredOn;
                adapter.SetAdapterState(!poweredOn);
            }
            else
            {
                Dialogs.Alert("Cannot change bluetooth adapter state");
            }
        }

        public string Title { get; private set; }
        public bool IsScanning { get; private set; }

    }
}