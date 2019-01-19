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
    class SignInViewModel: ViewModelBase
    {
        #region Fields And Properties

        string nickOrEmail;
        public string NickOrEmail { get => nickOrEmail; set => Set(ref nickOrEmail, value); }

        string password;
        public string Password { get => password; set => Set(ref password, value); }

        string errorMessage;
        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }

        #endregion


        #region Dependencies

        IAccountService accountService;
        INavigator navigator;

        #endregion

        #region Messages

        TripsViewModelMessage tripsViewModelMessage = new TripsViewModelMessage();

        #endregion

        #region Constuctors

        public SignInViewModel(INavigator navigator, IAccountService accountService)
        {
            this.accountService = accountService;
            this.navigator = navigator;
        }

        #endregion


        #region Commands

        private RelayCommand signInCommand;
        public RelayCommand SignInCommand
        {
            get => signInCommand ?? (signInCommand = new RelayCommand(()=>
            {
                var logInTask = Task.Run<(bool result, User foundUser)>(async () => await accountService.TryLogInAsync(nickOrEmail, password));

                logInTask.ContinueWith(t => {
                    if (logInTask.Result.result)
                    {
                        tripsViewModelMessage.User = logInTask.Result.foundUser;
                        Messenger.Default.Send<TripsViewModelMessage>(tripsViewModelMessage);
                        navigator.NavigateTo<TripsViewModel>();
                    }
                    else
                    {
                        ErrorMessage = "Error occured during signing in\n";
                    }
                }, TaskScheduler.Current);
            }));
        }

        private RelayCommand registerCommand;
        public RelayCommand RegisterCommand
        {
            get => registerCommand ?? (registerCommand = new RelayCommand(() => navigator.NavigateTo<RegisterViewModel>()));
        }

        #endregion
    }
}
