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
using TravelAppWpf.Extensions;
using TravelAppWpf.Messages;
using TravelAppWpf.Navigation;
using TravelAppWpf.Services.ProcessesInfo;

namespace TravelAppWpf.ViewModels
{
    class TripsViewModel : ViewModelBase
    {
        #region Fields And Properties

        private User user;

        private ObservableCollection<Trip> trips;
        public ObservableCollection<Trip> Trips { get => trips; set => Set(ref trips, value); }

        private string currentProcessesInfo;
        public string CurrentProcessesInfo
        {
            get { return currentProcessesInfo; }
            set
            {
                Set(ref currentProcessesInfo, value);
                
            }
        }

        #endregion


        #region Messages

        AddTripViewModelMessage addTripViewModelMessage = new AddTripViewModelMessage();
        TripDetailsObserverViewModelMessage tripDetailsObserverViewModelMessage = new TripDetailsObserverViewModelMessage();
        UpdateProcessInfoMessage updateProcessInfoMessage = new UpdateProcessInfoMessage();

        #endregion

        #region Dependencies

        INavigator navigator;
        private readonly IProcessesInfoService processesInfoService;
        ITripService tripService;

        #endregion


        #region Consturctors

        public TripsViewModel(INavigator navigator, IProcessesInfoService processInfoService, ITripService tripService)
        {
            this.navigator = navigator;
            this.processesInfoService = processInfoService;
            this.tripService = tripService;
            Messenger.Default.Register<TripsViewModelMessage>(this, m =>
            {
                user = m.User;
                UpdateTrips();
            });

            Messenger.Default.Register<UpdateTripsMessage>(this, m => UpdateTrips());
            Messenger.Default.Register<UpdateProcessInfoMessage>(this, m => UpdateCurrentProcessesInfo());
        }

        #endregion

        #region Commands

        private RelayCommand<Trip> deleteTripCommand;
        public RelayCommand<Trip> DeleteTripCommand
        {
            get => deleteTripCommand ?? (deleteTripCommand = new RelayCommand<Trip>(async t =>
            {
                int processId = processesInfoService.GenerateUniqueId();
                processesInfoService.ActivateProcess(ProcessEnum.DeletingTrip, processesInfoService.ProcessNames[ProcessEnum.DeletingTrip], processId);
                try
                {
                    Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                    await Task.Run(async () =>
                    {
                        await tripService.RemoveTripAsync(new DeleteByIdSpecification<Trip>(t.Id));
                        UpdateTrips();
                    });
                }
                finally
                {
                    processesInfoService.DeactivateProcess(ProcessEnum.DeletingTrip, processId);
                    Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                }
            }));
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
            get => signOutCommand ?? (signOutCommand = 
                new RelayCommand(() => navigator.NavigateTo<SignInViewModel>()));
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

        #region Private functions

        async void UpdateTrips()
        {
            await Task.Run(async () =>
           {
               Trips = new ObservableCollection<Trip>(await tripService.GetTripsOfUserAsync(user));
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

        
        #endregion
    }
}
