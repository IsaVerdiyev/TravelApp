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
    class AddTicketViewModel : ViewModelBase
    {
        #region Fields And Properties

        Trip trip;

        string ticketName;
        public string TicketName
        {
            get => ticketName;
            set
            {
                Set(ref ticketName, value);
                AddTicketCommand.RaiseCanExecuteChanged();
            }
        }

        string pdfPath;
        public string PdfPath
        {
            get => pdfPath;
            set
            {
                Set(ref pdfPath, value);
                AddTicketCommand.RaiseCanExecuteChanged();
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

        UpdateTicketsMessage updateTicketsMessage = new UpdateTicketsMessage();
        UpdateProcessInfoMessage updateProcessInfoMessage = new UpdateProcessInfoMessage();

        #endregion

        #region Dependencies

        private readonly INavigator navigator;
        private readonly IProcessesInfoService processesInfoService;
        private readonly ITicketService ticketService;

        #endregion

        #region Constructors

        public AddTicketViewModel(INavigator navigator, IProcessesInfoService processesInfoService, ITicketService ticketService)
        {
            this.navigator = navigator;
            this.processesInfoService = processesInfoService;
            this.ticketService = ticketService;

            Messenger.Default.Register<AddTicketViewModelMessage>(this, m => trip = m.Trip);

            Messenger.Default.Register<UpdateProcessInfoMessage>(this, m => UpdateCurrentProcessesInfo());
        }

        #endregion


        #region Commands

        RelayCommand addTicketCommand;
        public RelayCommand AddTicketCommand
        {
            get => addTicketCommand ?? (
                addTicketCommand = new RelayCommand(
                    async () =>
                    {

                        int processId = processesInfoService.GenerateUniqueId();
                        processesInfoService.ActivateProcess(ProcessEnum.AddingTicket, processesInfoService.ProcessNames[ProcessEnum.AddingTicket], processId);
                        try
                        {
                            Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                            await Task.Run(async () =>
                            {
                                await ticketService.AddTicketAsync(trip, new Ticket { Name = TicketName, ImagePath = PdfPath });
                                navigator.NavigateTo<TicketsViewModel>();
                                Messenger.Default.Send<UpdateTicketsMessage>(updateTicketsMessage);
                            });
                        }
                        finally
                        {
                            processesInfoService.DeactivateProcess(ProcessEnum.AddingTicket, processId);
                            Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                        }
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
