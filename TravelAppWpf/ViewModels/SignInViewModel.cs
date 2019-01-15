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
            get => signInCommand ?? (signInCommand = new RelayCommand(async() =>
            {
                var resultOfSigningIn = await accountService.TryLogInAsync(nickOrEmail, password);
                if (resultOfSigningIn.result)
                {
                    navigator.NavigateTo<TripsViewModel>();
                }
                else
                {
                    ErrorMessage = "Error occured during signing in\n";
                }
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
