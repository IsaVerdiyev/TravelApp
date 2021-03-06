﻿using GalaSoft.MvvmLight;
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

        private Dictionary<int, int> processKeysToTripsMap = new Dictionary<int, int>();

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
        private readonly IAccountService accountService;

        #endregion


        #region Consturctors

        public TripsViewModel(INavigator navigator, IProcessesInfoService processesInfoService, ITripService tripService, IAccountService accountService)
        {
            this.navigator = navigator;
            this.processesInfoService = processesInfoService;
            this.tripService = tripService;
            this.accountService = accountService;
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
                processKeysToTripsMap[t.Id] = processId;
                DeleteTripCommand.RaiseCanExecuteChanged();
                ObserveCommand.RaiseCanExecuteChanged();
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
                    processKeysToTripsMap.Remove(t.Id);
                }
            }
            , t => !processKeysToTripsMap.ContainsKey(t.Id)));
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
                , t => !processKeysToTripsMap.ContainsKey(t.Id)
                ));

        }

        private RelayCommand deleteUserCommand;
        public RelayCommand DeleteUserCommand
        {
            get => deleteUserCommand ?? (deleteUserCommand = new RelayCommand(async () =>
            {
                int processId = processesInfoService.GenerateUniqueId();
                processesInfoService.ActivateProcess(ProcessEnum.RemovingAccount, processesInfoService.ProcessNames[ProcessEnum.RemovingAccount], processId);
                try
                {
                    Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                    await Task.Run(async () =>
                    {
                        navigator.NavigateTo<SignInViewModel>();
                        await accountService.DeleteAccountAsync(new DeleteByIdSpecification<User>(user.Id));

                    });
                }
                finally
                {
                    processesInfoService.DeactivateProcess(ProcessEnum.RemovingAccount, processId);
                    Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                }

            }));
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
