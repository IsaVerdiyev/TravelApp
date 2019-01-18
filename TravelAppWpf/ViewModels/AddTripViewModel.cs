using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppWpf.Messages;
using TravelAppWpf.Navigation;

namespace TravelAppWpf.ViewModels
{
    class AddTripViewModel: ViewModelBase
    {
        #region Fields And Properties

        User user;

        string name;
        public string Name
        {
            get => name;
            set
            {
                Set(ref name, value);
                AddTripCommand.RaiseCanExecuteChanged();
            }
        }

        DateTime departureDate;
        public DateTime DepartureDate {
            get => departureDate;
            set
            {
                Set(ref departureDate, value);
                AddTripCommand.RaiseCanExecuteChanged();
            }
        }

        DateTime arrivalDate;
        public DateTime ArrivalDate {
            get => arrivalDate;
            set
            {
                Set(ref arrivalDate, value);
                AddTripCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion


        #region Dependencies

        private readonly INavigator navigator;
        private readonly ITripService tripService;

        #endregion


        #region Constructors

        public AddTripViewModel(INavigator navigator, ITripService tripService)
        {
            this.navigator = navigator;
            this.tripService = tripService;

            Messenger.Default.Register<AddTripViewModelMessage>(this, m => {
                user = m.User;
                DepartureDate = DateTime.Now;
                ArrivalDate = DateTime.Now.AddDays(3);
                });
        }

        #endregion


        #region Commands

        RelayCommand addTripCommand;
        public RelayCommand AddTripCommand
        {
            get => addTripCommand ?? (
                addTripCommand = new RelayCommand(async () =>
                {
                    Trip trip = new Trip
                    {
                        Name = Name,
                        ArriavalDate = ArrivalDate,
                        DepartureDate = DepartureDate,
                        CheckList = new List<ToDoItem>(),
                        Cities = new List<City>(),
                        Tickets = new List<Ticket>()
                    };

                    await tripService.AddTripAsync(user, trip);
                    navigator.NavigateTo<TripsViewModel>();

                }
                , () =>
                    {
                        return (DateTime.Compare(ArrivalDate, DepartureDate) >= 0 && !string.IsNullOrEmpty(Name));
                    }
                ));
        }

        RelayCommand returnBackCommand;
        public RelayCommand ReturnBackCommand
        {
            get => returnBackCommand ?? (returnBackCommand = new RelayCommand(() => navigator.NavigateTo<TripsViewModel>()));
        }

        #endregion




    }
}
