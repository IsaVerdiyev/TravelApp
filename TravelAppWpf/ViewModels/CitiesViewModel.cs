using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppCore.Specifications;
using TravelAppWpf.Messages;
using TravelAppWpf.Navigation;

namespace TravelAppWpf.ViewModels
{
    class CitiesViewModel: ViewModelBase
    {
        #region Fields And Properties

        User user;

        Trip trip;

        public ObservableCollection<City> Cities { get => new ObservableCollection<City>(cityService.GetCitiesOfTrip(trip)); }

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

            Messenger.Default.Register<TripDetailsObserverViewModelMessage>(this, m => {
                user = m.User;
                trip = m.Trip;
            });
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
            get => addCityCommand ?? (addCityCommand = new RelayCommand(() => navigator.NavigateTo<AddCityViewModel>()));
        }

        RelayCommand<City> deleteCityCommand;
        public RelayCommand<City> DeleteCityCommand
        {
            get => deleteCityCommand ?? (deleteCityCommand = new RelayCommand<City>(async c => await cityService.RemoveCityAsync(new DeleteByIdSpecification<City>(c.Id))));
        }


        #endregion
    }
}
