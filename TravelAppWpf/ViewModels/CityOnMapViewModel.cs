using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppWpf.Messages;
using TravelAppWpf.Navigation;

namespace TravelAppWpf.ViewModels
{
    class CityOnMapViewModel: ViewModelBase
    {

        #region Fields And Properties

        User user;
        Trip trip;

        City city;
        public City City { get => city; set => Set(ref city, value); }

        #endregion

        #region Dependencies
        private readonly INavigator navigator;
        private readonly ICityCoordinateGetter cityCoordinateGetter;
        #endregion

        #region Constructors

        public CityOnMapViewModel(INavigator navigator, ICityCoordinateGetter cityCoordinateGetter)
        {
            this.navigator = navigator;
            this.cityCoordinateGetter = cityCoordinateGetter;
            Messenger.Default.Register<CityOnMapViewModelMessage>(this, async m => {
                user = m.User;
                trip = m.Trip;
                m.City.CityCoordinate = await this.cityCoordinateGetter.GetCityCoordinateOfCityAsync(m.City);
                City = m.City;
            });
        }

        #endregion


        #region Commands

        RelayCommand returnBackCommand;
        public RelayCommand ReturnBackCommand
        {
            get => returnBackCommand ?? (returnBackCommand = new RelayCommand(() => navigator.NavigateTo<CitiesViewModel>()));
        }

        #endregion
    }
}
