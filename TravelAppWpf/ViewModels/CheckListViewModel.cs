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

namespace TravelAppWpf.ViewModels
{
    class CheckListViewModel:  ViewModelBase
    {
        #region Fields And Properties

        User user;

        Trip trip;

        private ObservableCollection<ToDoItem> checkList;

        public ObservableCollection<ToDoItem> CheckList { get => checkList; set => Set(ref checkList, value); }

        #endregion

        #region Messages

        AddToDoItemViewModelMessage addToDoItemViewModelMessage = new AddToDoItemViewModelMessage();

        #endregion

        #region Dependencies

        INavigator navigator;
        ICheckListService checkListService;


        #endregion

        #region Constructors

        public CheckListViewModel(INavigator navigator, ICheckListService checkListService)
        {
            this.navigator = navigator;
            this.checkListService = checkListService;

            Messenger.Default.Register<TripDetailsObserverViewModelMessage>(this, m => {
                user = m.User;
                trip = m.Trip;
            });
        }

        #endregion


        #region Commands

        private RelayCommand<ToDoItem> changeToDoItemStateCommand;
        public RelayCommand<ToDoItem> ChangeToDoItemStateCommand
        {
            get => changeToDoItemStateCommand ?? (changeToDoItemStateCommand = new RelayCommand<ToDoItem>(
                        async p => {
                            await Task.Run(async () => {
                                p = await checkListService.ChangeCheckedStateOfToDoItemAsync(p, !p.Done);
                                //checkList.First(i => i.Id == p.Id).Done = p.Done;
                            });
                        }
                        ));
        }

        private RelayCommand addToDoItemInCheckListCommand;
        public RelayCommand AddToDoItemInCheckListCommand
        {
            get => addToDoItemInCheckListCommand ?? (addToDoItemInCheckListCommand = new RelayCommand(
                        () => {
                            addToDoItemViewModelMessage.User = user;
                            addToDoItemViewModelMessage.Trip = trip;
                            Messenger.Default.Send<AddToDoItemViewModelMessage>(addToDoItemViewModelMessage);
                            navigator.NavigateTo<AddToDoItemViewModel>();
                        }
                        ));
        }

        private RelayCommand<ToDoItem> deleteToDoItemCommand;
        public RelayCommand<ToDoItem> DeleteToDoItemCommand
        {
            get => deleteToDoItemCommand ?? (deleteToDoItemCommand = new RelayCommand<ToDoItem>(
                        async p => {
                            await Task.Run(async () => {
                                await checkListService.RemoveItemFromCheckListAsync(new DeleteByIdSpecification<ToDoItem>(p.Id));
                                CheckList = new ObservableCollection<ToDoItem>(await checkListService.GetCheckListOfTripAsync(trip));
                            });
                        }
                        ));
        }



        private RelayCommand returnBackCommand;
        public RelayCommand ReturnBackCommand
        {
            get => returnBackCommand ?? (returnBackCommand = new RelayCommand(
                        () => navigator.NavigateTo<TripsViewModel>()
                        ));
        }




        #endregion


    }
}
