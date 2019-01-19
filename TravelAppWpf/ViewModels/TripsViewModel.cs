using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualBasic;
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
    class TripsViewModel: ViewModelBase
    {
        #region Fields And Properties

        private User user;

       
        public ObservableCollection<Trip> Trips
        {
            get
            {
                var tripsGettingTask = Task.Run(async() => await tripService.GetTripsOfUserAsync(user));
                return new ObservableCollection<Trip>(tripsGettingTask.ContinueWith(t => t.Result, TaskScheduler.Current).Result);
                
            }
        }


        #endregion


        #region Messages

        AddTripViewModelMessage addTripViewModelMessage = new AddTripViewModelMessage();
        TripDetailsObserverViewModelMessage tripDetailsObserverViewModelMessage = new TripDetailsObserverViewModelMessage();

        #endregion

        #region Dependencies

        INavigator navigator;
        ITripService tripService;

        #endregion


        #region Consturctors

        public TripsViewModel(INavigator navigator, ITripService tripService)
        {
            this.navigator = navigator;
            this.tripService = tripService;
            Messenger.Default.Register<TripsViewModelMessage>(this, m =>
            {
                user = m.User;
                
            }, true);
        }

        #endregion

        #region Commands

        private RelayCommand<Trip> deleteTripCommand;
        public RelayCommand<Trip> DeleteTripCommand
        {
            get => deleteTripCommand ?? (deleteTripCommand = new RelayCommand<Trip>(
                        t =>
                        {
                            var removeTripTask = Task.Run(async () => await tripService.RemoveTripAsync(new DeleteByIdSpecification<Trip>(t.Id)));
                            removeTripTask.ContinueWith(task => RaisePropertyChanged(nameof(Trips)), TaskScheduler.Current);
                        }
                        ));
        }


        private RelayCommand addTripCommand;
        public RelayCommand AddTripCommand
        {
            get => addTripCommand ?? (addTripCommand = new RelayCommand(() =>
            {
                addTripViewModelMessage.User = user;
                Messenger.Default.Send(addTripViewModelMessage);
                navigator.NavigateTo<AddTripViewModel>();
            }));
        }


        private RelayCommand signOutCommand;
        public RelayCommand SignOutCommand
        {
            get => signOutCommand ?? (signOutCommand = new RelayCommand(
                        () => navigator.NavigateTo<SignInViewModel>()
                        ));
        }


        private RelayCommand<Trip> observeCommand;
        public RelayCommand<Trip> ObserveCommand
        {
            get => observeCommand ?? (
                observeCommand = new RelayCommand<Trip>(t =>
                {
                    tripDetailsObserverViewModelMessage.User = user;
                    tripDetailsObserverViewModelMessage.Trip = t;
                    Messenger.Default.Send<TripDetailsObserverViewModelMessage>(tripDetailsObserverViewModelMessage);
                    navigator.NavigateTo<CitiesViewModel>();
                }
                ));
              
        }

        #endregion
    }
}
