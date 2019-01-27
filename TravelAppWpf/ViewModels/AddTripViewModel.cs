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
using TravelAppWpf.Extensions;
using TravelAppWpf.Messages;
using TravelAppWpf.Navigation;
using TravelAppWpf.Services.ProcessesInfo;

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

        UpdateTripsMessage updateTripsMessage = new UpdateTripsMessage();
        UpdateProcessInfoMessage updateProcessInfoMessage = new UpdateProcessInfoMessage();

        #endregion

        #region Dependencies

        private readonly INavigator navigator;
        private readonly IProcessesInfoService processesInfoService;
        private readonly ITripService tripService;

        #endregion


        #region Constructors

        public AddTripViewModel(INavigator navigator, IProcessesInfoService processesInfoService, ITripService tripService)
        {
            this.navigator = navigator;
            this.processesInfoService = processesInfoService;
            this.tripService = tripService;

            Messenger.Default.Register<AddTripViewModelMessage>(this, m => {
                user = m.User;
                Name = "";
                DepartureDate = DateTime.Now;
                ArrivalDate = DateTime.Now.AddDays(3);
                }
            );

            Messenger.Default.Register<UpdateProcessInfoMessage>(this, m => UpdateCurrentProcessesInfo());
        }

        #endregion


        #region Commands

        RelayCommand addTripCommand;
        public RelayCommand AddTripCommand
        {
            get => addTripCommand ?? (
                addTripCommand = new RelayCommand(async() =>
                {
                int processId = processesInfoService.GenerateUniqueId();
                processesInfoService.ActivateProcess(ProcessEnum.AddingTrip, processesInfoService.ProcessNames[ProcessEnum.AddingTrip], processId);
                    try
                    {
                        Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                        await Task.Run(async () =>
                        {
                            navigator.NavigateTo<TripsViewModel>();
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
                            Messenger.Default.Send<UpdateTripsMessage>(updateTripsMessage);

                        });
                    }
                    finally
                    {
                        processesInfoService.DeactivateProcess(ProcessEnum.AddingTrip, processId);
                        Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                    }

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


        #region Private Functions

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
