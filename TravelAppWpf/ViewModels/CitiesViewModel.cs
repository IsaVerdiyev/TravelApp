using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppCore.Specifications;
using TravelAppWpf.Messages;
using TravelAppWpf.Navigation;

namespace TravelAppWpf.ViewModels
{
    class CitiesViewModel : ViewModelBase
    {
        #region Fields And Properties

        User user;

        Trip trip;


        ObservableCollection<City> cities;
        public ObservableCollection<City> Cities
        {
            get => cities;
            set => Set(ref cities, value);
        }

        #endregion


        #region Messages

        CityOnMapViewModelMessage cityOnMapViewModelMessage = new CityOnMapViewModelMessage();
        AddCityViewModelMessage addCityViewModelMessage = new AddCityViewModelMessage();
        #endregion

        #region Dependencies

        private readonly INavigator navigator;
        private readonly ICityService cityService;

        #endregion

        #region Constructors

        public CitiesViewModel(INavigator navigator, ICityService cityService)
        {
            this.navigator = navigator;
            this.cityService = cityService;

            Messenger.Default.Register<TripDetailsObserverViewModelMessage>(this, m =>
            {
                user = m.User;
                trip = m.Trip;
                UpdateCities();
            });

            Messenger.Default.Register<UpdateCitiesMessage>(this, UpdateCitiesOnMessage, true);
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

        RelayCommand<City> deleteCityCommand;
        public RelayCommand<City> DeleteCityCommand
        {
            get => deleteCityCommand ?? (deleteCityCommand = new RelayCommand<City>(c =>
            {
                var removeCityTask = Task.Run(() => cityService.RemoveCity(new DeleteByIdSpecification<City>(c.Id)));
                removeCityTask.ContinueWith(t => Cities = new ObservableCollection<City>(cityService.GetCitiesOfTrip(trip)), CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Current);
            }));
        }

        RelayCommand<City> showInfoOfCity;
        public RelayCommand<City> ShowInfoOfCity
        {
            get => showInfoOfCity ?? (showInfoOfCity = new RelayCommand<City>(c =>
            {
                cityOnMapViewModelMessage.User = user;
                cityOnMapViewModelMessage.Trip = trip;
                cityOnMapViewModelMessage.City = c;

                Messenger.Default.Send<CityOnMapViewModelMessage>(cityOnMapViewModelMessage);

                navigator.NavigateTo<CityOnMapViewModel>();
            }));
        }

        #endregion


        #region Private Functions


        void UpdateCities()
        {
            Task<IReadOnlyList<City>> getCitiesTask = Task.Run<IReadOnlyList<City>>(() => cityService.GetCitiesOfTrip(trip));
            getCitiesTask.ContinueWith(t =>
            {
                Cities = new ObservableCollection<City>(t.Result);
                //RaisePropertyChanged(nameof(Cities));
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Current);
            getCitiesTask.ContinueWith(t => MessageBox.Show(t.Exception.InnerExceptions.First().Message), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
        }
        void UpdateCitiesOnMessage(UpdateCitiesMessage updateCitiesMessage)
        {
            UpdateCities();
        }

        #endregion
    }
}
