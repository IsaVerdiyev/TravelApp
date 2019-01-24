﻿using GalaSoft.MvvmLight;
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

        private City selectedCity;
        public City SelectedCity{  get => selectedCity; set => Set(ref selectedCity, value); }

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

            Messenger.Default.Register<UpdateCitiesMessage>(this, m => UpdateCities());
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
            get => deleteCityCommand ?? (deleteCityCommand = new RelayCommand<City>(async c =>
            {
                await Task.Run(async () =>
                {
                    await cityService.RemoveCityAsync(new DeleteByIdSpecification<City>(c.Id));
                    UpdateCities();
                });
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
            await Task.Run(async() =>
            {
                Cities = new ObservableCollection<City>(await cityService.GetCitiesOfTripAsync(trip));
            });
        }

        #endregion
    }
}
