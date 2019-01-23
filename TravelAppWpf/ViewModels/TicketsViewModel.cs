using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppWpf.Navigation;
using TravelAppWpf.Views;

namespace TravelAppWpf.ViewModels
{
    class TicketsViewModel: ViewModelBase
    {
        #region Fields And Properties

        ObservableCollection<Ticket> tickets;
        public ObservableCollection<Ticket> Tickets { get => tickets; set => Set(ref tickets, value); }

        #endregion


        #region Dependencies

        INavigator navigator;

        #endregion

        #region Constructors

        public TicketsViewModel(INavigator navigator)
        {
            this.navigator = navigator;
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
            get => addTicketCommand = new RelayCommand(() => {
                AddTicketView addTicketView = new AddTicketView();
                addTicketView.ShowDialog();
            });
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
    }
}
