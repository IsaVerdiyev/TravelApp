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
using TravelAppWpf.Views;

namespace TravelAppWpf.ViewModels
{
    class TicketsViewModel: ViewModelBase
    {
        #region Fields And Properties

        ObservableCollection<Ticket> tickets;
        public ObservableCollection<Ticket> Tickets { get => tickets; set => Set(ref tickets, value); }

        User user;
        Trip trip;

        #endregion

        #region Messages

        AddTicketViewModelMessage addTicketViewModelMessage = new AddTicketViewModelMessage();

        #endregion

        #region Dependencies

        private readonly INavigator navigator;
        private readonly ITicketService ticketService;

        #endregion

        #region Constructors

        public TicketsViewModel(INavigator navigator, ITicketService ticketService)
        {
            this.navigator = navigator;
            this.ticketService = ticketService;
            Messenger.Default.Register<TripDetailsObserverViewModelMessage>(this, m => {
                user = m.User;
                trip = m.Trip;
                addTicketViewModelMessage.Trip = trip;
                UpdateTickets();
            });

            Messenger.Default.Register<UpdateTicketsMessage>(this, m => UpdateTickets());
        }

        #endregion

        #region Commands

        RelayCommand returnBackCommand;
        public RelayCommand ReturnCommand
        {
            get => returnBackCommand = new RelayCommand(() => navigator.NavigateTo<TripsViewModel>());
        }


        RelayCommand addTicketCommand;
        public RelayCommand AddTicketCommand
        {
            get => addTicketCommand ?? (addTicketCommand = new RelayCommand(() => {
                AddTicketView addTicketView = new AddTicketView();
                Messenger.Default.Send<AddTicketViewModelMessage>(addTicketViewModelMessage);
                addTicketView.ShowDialog();
            }));
        }

        RelayCommand<Ticket> deleteTicketCommand;
        public RelayCommand<Ticket> DeleteTicketCommand
        {
            get => deleteTicketCommand ?? (deleteTicketCommand = new RelayCommand<Ticket>(async t => {
                await Task.Run(async() => {
                    await ticketService.RemoveTicketAsync(new DeleteByIdSpecification<Ticket>(t.Id));
                    UpdateTickets();
                });
            }));
        }

        private RelayCommand navigateToCheckListCommand;
        public RelayCommand NavigateToCheckListCommand
        {
            get => navigateToCheckListCommand ?? (navigateToCheckListCommand = new RelayCommand(
                        () => navigator.NavigateTo<CheckListViewModel>()
                        ));
        }

        private RelayCommand navigateToCitiesCommand;
        public RelayCommand NavigateToCitiesCommand
        {
            get => navigateToCitiesCommand ?? (navigateToCitiesCommand = new RelayCommand(
                        () => navigator.NavigateTo<CitiesViewModel>()
                        ));
        }
        #endregion


        #region

        async void UpdateTickets()
        {
            await Task.Run(async () =>
            {
                Tickets = new ObservableCollection<Ticket>(await ticketService.GetTicketsOfTripAsync(trip));
            });
        }

        #endregion
    }
}
