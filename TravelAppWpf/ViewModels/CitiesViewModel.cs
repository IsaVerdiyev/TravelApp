using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppCore.Specifications;
using TravelAppWpf.Extensions;
using TravelAppWpf.Messages;
using TravelAppWpf.Navigation;
using TravelAppWpf.Services.ProcessesInfo;

namespace TravelAppWpf.ViewModels
{
    class CitiesViewModel : ViewModelBase
    {
        #region Fields And Properties

        User user;

        Trip trip;


        ObservableCollection<DestinationCityInTrip> destinationsInTrip;
        public ObservableCollection<DestinationCityInTrip> Destinations
        {
            get => destinationsInTrip;
            set => Set(ref destinationsInTrip, value);
        }

        private City selectedCity;
        public City SelectedCity { get => selectedCity; set => Set(ref selectedCity, value); }

        private string currentProcessesInfo;
        public string CurrentProcessesInfo
        {
            get { return currentProcessesInfo; }
            set
            {
                Set(ref currentProcessesInfo, value);

            }
        }

        private Dictionary<int, int> processKeysToDestinationsMap = new Dictionary<int, int>();
        #endregion


        #region Messages

        CityOnMapViewModelMessage cityOnMapViewModelMessage = new CityOnMapViewModelMessage();
        AddCityViewModelMessage addCityViewModelMessage = new AddCityViewModelMessage();
        UpdateProcessInfoMessage updateProcessInfoMessage = new UpdateProcessInfoMessage();
        #endregion

        #region Dependencies

        private readonly INavigator navigator;
        private readonly IProcessesInfoService processesInfoService;
        private readonly IDestinationsInTripService destinationsInTripService;

        #endregion

        #region Constructors

        public CitiesViewModel(INavigator navigator, IProcessesInfoService processesInfoService, IDestinationsInTripService destinationsInTripService)
        {
            this.navigator = navigator;
            this.processesInfoService = processesInfoService;
            this.destinationsInTripService = destinationsInTripService;
            Messenger.Default.Register<TripDetailsObserverViewModelMessage>(this, m =>
            {
                user = m.User;
                trip = m.Trip;
                UpdateCities();
            });

            Messenger.Default.Register<UpdateCitiesMessage>(this, m => UpdateCities());
            Messenger.Default.Register<UpdateProcessInfoMessage>(this, m => UpdateCurrentProcessesInfo());
        }

        #endregion


        #region Commands

        RelayCommand returnBackCommand;
        public RelayCommand ReturnBackCommand
        {
            get => returnBackCommand ?? (returnBackCommand = new RelayCommand(() => navigator.NavigateTo<TripsViewModel>()));
        }


        RelayCommand addCityCommand;
        public RelayCommand AddCityCommand
        {
            get => addCityCommand ?? (addCityCommand = new RelayCommand(() =>
            {
                addCityViewModelMessage.User = user;
                addCityViewModelMessage.Trip = trip;
                Messenger.Default.Send<AddCityViewModelMessage>(addCityViewModelMessage);
                navigator.NavigateTo<AddCityViewModel>();
            }));
        }

        RelayCommand<DestinationCityInTrip> deleteCityCommand;
        public RelayCommand<DestinationCityInTrip> DeleteCityCommand
        {
            get => deleteCityCommand ?? (deleteCityCommand = new RelayCommand<DestinationCityInTrip>(async d =>
            {
                int processId = processesInfoService.GenerateUniqueId();
                processesInfoService.ActivateProcess(ProcessEnum.DeletingCity, processesInfoService.ProcessNames[ProcessEnum.DeletingCity], processId);
                processKeysToDestinationsMap[d.Id] = processId;
                DeleteCityCommand.RaiseCanExecuteChanged();
                ShowInfoOfCityCommand.RaiseCanExecuteChanged();
                try
                {
                    Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                    await Task.Run(async () =>
                    {
                        await destinationsInTripService.RemoveDestinationFromTripAsync(new DeleteByIdSpecification<DestinationCityInTrip>(d.Id));
                        UpdateCities();
                    });
                }
                finally
                {
                    processesInfoService.DeactivateProcess(ProcessEnum.DeletingCity, processId);
                    Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                    processKeysToDestinationsMap.Remove(d.Id);
                }
            },
            c => !CheckProcess(c.Id)));
        }

        RelayCommand<DestinationCityInTrip> showInfoOfCityCommand;
        public RelayCommand<DestinationCityInTrip> ShowInfoOfCityCommand
        {
            get => showInfoOfCityCommand ?? (showInfoOfCityCommand = new RelayCommand<DestinationCityInTrip>(c =>
            {
                cityOnMapViewModelMessage.User = user;
                cityOnMapViewModelMessage.Trip = trip;
                cityOnMapViewModelMessage.City = c.City;

                Messenger.Default.Send<CityOnMapViewModelMessage>(cityOnMapViewModelMessage);

                navigator.NavigateTo<CityOnMapViewModel>();
            },
            c => !CheckProcess(c.Id)));
        }

        private RelayCommand navigateToCheckListCommand;
        public RelayCommand NavigateToCheckListCommand
        {
            get => navigateToCheckListCommand ?? (navigateToCheckListCommand = new RelayCommand(
                        () => navigator.NavigateTo<CheckListViewModel>()
                        ));
        }

        RelayCommand navigateToTicketsCommand;
        public RelayCommand NavigateToTicketsCommand
        {
            get => navigateToTicketsCommand ?? (navigateToTicketsCommand = new RelayCommand(
                () => navigator.NavigateTo<TicketsViewModel>()
                ));
        }


        #endregion


        #region Private Functions


        async void UpdateCities()
        {
            await Task.Run(async () =>
            {
                Destinations = new ObservableCollection<DestinationCityInTrip>(await destinationsInTripService.GetDestinationsOfTripAsync(trip));
            });
        }

        void UpdateCurrentProcessesInfo()
        {
            try
            {
                CurrentProcessesInfo = processesInfoService.GetOneInfoStringFromAllProcesses();
            }
            catch (InvalidOperationException ex)
            {
                CurrentProcessesInfo = "";
            }
        }

        bool CheckProcess(int id)
        {
            return processKeysToDestinationsMap.ContainsKey(id);
        }

        #endregion
    }
}
