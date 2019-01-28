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
    class AddTicketViewModel: ViewModelBase
    {
        #region Fields And Properties

        Trip trip;

        string ticketName;
        public string TicketName {
            get => ticketName;
            set
            {
                Set(ref ticketName, value);
                AddTicketCommand.RaiseCanExecuteChanged();
            }
        }

        string pdfPath;
        public string PdfPath {
            get => pdfPath;
            set
            {
                Set(ref pdfPath, value);
                AddTicketCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Messages

        UpdateTicketsMessage updateTicketsMessage = new UpdateTicketsMessage();
        private readonly INavigator navigator;

        #endregion

        #region Dependencies

        ITicketService ticketService;

        #endregion

        #region Constructors

        public AddTicketViewModel(INavigator navigator, ITicketService ticketService)
        {
            this.navigator = navigator;
            this.ticketService = ticketService;

            Messenger.Default.Register<AddTicketViewModelMessage>(this, m => trip = m.Trip);
        }

        #endregion


        #region Commands

        RelayCommand addTicketCommand;
        public RelayCommand AddTicketCommand
        {
            get => addTicketCommand ?? (
                addTicketCommand = new RelayCommand(
                    async () => {
                        await Task.Run(async() => {
                            await ticketService.AddTicketAsync(trip, new Ticket {Name = TicketName,ImagePath = PdfPath});
                            navigator.NavigateTo<TicketsViewModel>();
                            Messenger.Default.Send<UpdateTicketsMessage>(updateTicketsMessage);
                        });
                    },
                    () => !string.IsNullOrWhiteSpace(TicketName) && !string.IsNullOrWhiteSpace(PdfPath))
                );
        }


        RelayCommand returnCommand;
        public RelayCommand ReturnCommand
        {
            get => returnCommand ?? (returnCommand = new RelayCommand(() => navigator.NavigateTo<TicketsViewModel>()));
        }
        #endregion
    }
}
