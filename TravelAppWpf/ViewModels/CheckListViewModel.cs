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
using System.Windows;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppCore.Specifications;
using TravelAppWpf.Extensions;
using TravelAppWpf.Messages;
using TravelAppWpf.Navigation;
using TravelAppWpf.Services.ProcessesInfo;

namespace TravelAppWpf.ViewModels
{
    class CheckListViewModel : ViewModelBase
    {
        #region Fields And Properties

        User user;

        Trip trip;

        private ObservableCollection<ToDoItem> checkList;
        public ObservableCollection<ToDoItem> CheckList { get => checkList; set => Set(ref checkList, value); }


        private string currentProcessesInfo;
        public string CurrentProcessesInfo
        {
            get { return currentProcessesInfo; }
            set
            {
                Set(ref currentProcessesInfo, value);

            }
        }

        private Dictionary<int, int> toDoItemsIdToDeletingProcessMap = new Dictionary<int, int>();
        private Dictionary<int, int> toDoItemsIdToModifyingProcessesMap = new Dictionary<int, int>();
        #endregion

        #region Messages

        AddToDoItemViewModelMessage addToDoItemViewModelMessage = new AddToDoItemViewModelMessage();
        UpdateProcessInfoMessage updateProcessInfoMessage = new UpdateProcessInfoMessage();
        #endregion

        #region Dependencies

        INavigator navigator;
        private readonly IProcessesInfoService processesInfoService;
        ICheckListService checkListService;


        #endregion

        #region Constructors

        public CheckListViewModel(INavigator navigator, IProcessesInfoService processesInfoService, ICheckListService checkListService)
        {
            this.navigator = navigator;
            this.processesInfoService = processesInfoService;
            this.checkListService = checkListService;

            Messenger.Default.Register<TripDetailsObserverViewModelMessage>(this, m =>
            {
                user = m.User;
                trip = m.Trip;
                UpdateCheckList();
            });

            Messenger.Default.Register<UpdateCheckListMessage>(this, m => UpdateCheckList());

            Messenger.Default.Register<UpdateProcessInfoMessage>(this, m => UpdateCurrentProcessesInfo());
        }

        #endregion


        #region Commands

        private RelayCommand<ToDoItem> changeToDoItemStateCommand;
        public RelayCommand<ToDoItem> ChangeToDoItemStateCommand
        {
            get => changeToDoItemStateCommand ?? (changeToDoItemStateCommand = new RelayCommand<ToDoItem>(
                        async p =>
                        {
                            int processId = processesInfoService.GenerateUniqueId();
                            processesInfoService.ActivateProcess(ProcessEnum.ModifyingItemInCheckList, processesInfoService.ProcessNames[ProcessEnum.ModifyingItemInCheckList], processId);
                            toDoItemsIdToModifyingProcessesMap[p.Id] = processId;
                            ChangeToDoItemStateCommand.RaiseCanExecuteChanged();
                            try
                            {
                                Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                                await Task.Run(async () =>
                                {
                                    p = await checkListService.ChangeCheckedStateOfToDoItemAsync(p, p.Done);
                                });
                            }
                            finally
                            {
                                processesInfoService.DeactivateProcess(ProcessEnum.ModifyingItemInCheckList, processId);
                                Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                                toDoItemsIdToModifyingProcessesMap.Remove(p.Id);
                                ChangeToDoItemStateCommand.RaiseCanExecuteChanged();
                            }
                        },
                        p => !toDoItemsIdToDeletingProcessMap.ContainsKey(p.Id) &&
                             !toDoItemsIdToModifyingProcessesMap.ContainsKey(p.Id)
                        ));
        }

        private RelayCommand addToDoItemInCheckListCommand;
        public RelayCommand AddToDoItemInCheckListCommand
        {
            get => addToDoItemInCheckListCommand ?? (addToDoItemInCheckListCommand = new RelayCommand(
                        () =>
                        {
                            addToDoItemViewModelMessage.Trip = trip;
                            addToDoItemViewModelMessage.User = user;
                            Messenger.Default.Send<AddToDoItemViewModelMessage>(addToDoItemViewModelMessage);
                            navigator.NavigateTo<AddItemInCheckListViewModel>();
                        }
                        ));
        }

        private RelayCommand<ToDoItem> deleteToDoItemCommand;
        public RelayCommand<ToDoItem> DeleteToDoItemCommand
        {
            get => deleteToDoItemCommand ?? (deleteToDoItemCommand = new RelayCommand<ToDoItem>(
                        async p =>
                        {
                            int processId = processesInfoService.GenerateUniqueId();
                            processesInfoService.ActivateProcess(ProcessEnum.DeletingItemFromCheckList, processesInfoService.ProcessNames[ProcessEnum.DeletingItemFromCheckList], processId);
                            toDoItemsIdToDeletingProcessMap[p.Id] = processId;
                            DeleteToDoItemCommand.RaiseCanExecuteChanged();
                            ChangeToDoItemStateCommand.RaiseCanExecuteChanged();
                            try
                            {
                                Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                                await Task.Run(async () =>
                                {
                                    await checkListService.RemoveItemFromCheckListAsync(new DeleteByIdSpecification<ToDoItem>(p.Id));
                                    UpdateCheckList();
                                });
                            }
                            finally
                            {
                                processesInfoService.DeactivateProcess(ProcessEnum.DeletingItemFromCheckList, processId);
                                Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                                toDoItemsIdToDeletingProcessMap.Remove(p.Id);
                            }
                        },
                        p => !toDoItemsIdToDeletingProcessMap.ContainsKey(p.Id)
                        ));
        }



        private RelayCommand returnBackCommand;
        public RelayCommand ReturnBackCommand
        {
            get => returnBackCommand ?? (returnBackCommand = new RelayCommand(
                        () => navigator.NavigateTo<TripsViewModel>()
                        ));
        }

        private RelayCommand navigateToCitiesCommand;
        public RelayCommand NavigateToCitiesCommand
        {
            get => navigateToCitiesCommand ?? (navigateToCitiesCommand = new RelayCommand(
                        () => navigator.NavigateTo<CitiesViewModel>()
                        ));
        }

        RelayCommand navigateToTicketsCommand;
        public RelayCommand NavigateToTicketsCommand
        {
            get => navigateToTicketsCommand ?? (navigateToTicketsCommand = new RelayCommand(
                () => navigator.NavigateTo<TicketsViewModel>()
                ));
        }




        #endregion


        #region Private Functions

        async void UpdateCheckList()
        {
            await Task.Run(async () =>
            {
                CheckList = new ObservableCollection<ToDoItem>(await checkListService.GetCheckListOfTripAsync(trip));
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
