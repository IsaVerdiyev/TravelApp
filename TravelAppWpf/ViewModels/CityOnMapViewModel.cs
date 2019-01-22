using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
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

        //public string LatLob { get => $"{city}"}

        #endregion

        #region Dependencies
        private readonly INavigator navigator;
        #endregion

        #region Constructors

        public CityOnMapViewModel(INavigator navigator)
        {
            this.navigator = navigator;

            Messenger.Default.Register<CityOnMapViewModelMessage>(this, m => {
                user = m.User;
                trip = m.Trip;
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
