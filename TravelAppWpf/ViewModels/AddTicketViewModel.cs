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

        #endregion

        #region Dependencies

        ITicketService ticketService;

        #endregion

        #region Constructors

        public AddTicketViewModel(ITicketService ticketService)
        {
            this.ticketService = ticketService;

            Messenger.Default.Register<AddTicketViewModelMessage>(this, m => trip = m.Trip);
        }

        #endregion


        #region Commands

        RelayCommand<Window> addTicketCommand;
        public RelayCommand<Window> AddTicketCommand
        {
            get => addTicketCommand ?? (
                addTicketCommand = new RelayCommand<Window>(
                    async w => {
                        w.Close();
                        await Task.Run(async() => {
                            await ticketService.AddTicketAsync(trip, new Ticket {Name = TicketName,ImagePath = PdfPath});
                            Messenger.Default.Send<UpdateTicketsMessage>(updateTicketsMessage);
                        });
                    },
                    w => !string.IsNullOrWhiteSpace(TicketName) && !string.IsNullOrWhiteSpace(PdfPath))
                );
        }

        #endregion
    }
}
