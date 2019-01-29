using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppWpf.Extensions;
using TravelAppWpf.Messages;
using TravelAppWpf.Navigation;
using TravelAppWpf.Services.ProcessesInfo;

namespace TravelAppWpf.ViewModels
{
    class AddItemInCheckListViewModel : ViewModelBase
    {
        #region Fields And Properties

        Trip trip;

        string name;
        public string Name {
            get => name;
            set
            {
                Set(ref name, value);
                AddItemInCheckListCommand.RaiseCanExecuteChanged();
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

        UpdateCheckListMessage updateCheckListMessage = new UpdateCheckListMessage();
        UpdateProcessInfoMessage updateProcessInfoMessage = new UpdateProcessInfoMessage();

        #endregion


        #region Dependencies

        private readonly INavigator navigator;
        private readonly IProcessesInfoService processesInfoService;
        private readonly ICheckListService checkListService;


        #endregion


        #region Constructors

        public AddItemInCheckListViewModel(INavigator navigator, IProcessesInfoService processesInfoService, ICheckListService checkListService)
        {
            this.navigator = navigator;
            this.processesInfoService = processesInfoService;
            this.checkListService = checkListService;

            Messenger.Default.Register<AddToDoItemViewModelMessage>(this, m => trip = m.Trip);
            Messenger.Default.Register<UpdateProcessInfoMessage>(this, m => UpdateCurrentProcessesInfo());
        }

        #endregion

        #region Commands

        RelayCommand returnBackCommand;
        public RelayCommand ReturnBackCommand
        {
            get => returnBackCommand ?? (returnBackCommand = new RelayCommand(() => navigator.NavigateTo<TicketsViewModel>()));
        }

        RelayCommand addItemInCheckListCommand;
        public RelayCommand AddItemInCheckListCommand
        {
            get => addItemInCheckListCommand ?? (addItemInCheckListCommand = new RelayCommand(async () =>
            {
                int processId = processesInfoService.GenerateUniqueId();
                processesInfoService.ActivateProcess(ProcessEnum.AddingItemInCheckList, processesInfoService.ProcessNames[ProcessEnum.AddingItemInCheckList], processId);
                try
                {
                    Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                    await Task.Run(async () =>
                    {
                        await checkListService.AddItemInCheckListAsync(trip, new ToDoItem { Name = Name });
                        Messenger.Default.Send<UpdateCheckListMessage>(updateCheckListMessage);
                        navigator.NavigateTo<CheckListViewModel>();
                    });
                }
                finally
                {
                    processesInfoService.DeactivateProcess(ProcessEnum.AddingItemInCheckList, processId);
                    Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                }
            },
                () => !string.IsNullOrWhiteSpace(Name)));
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
