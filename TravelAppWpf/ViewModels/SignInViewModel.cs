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
using TravelAppWpf.Services.ProcessesInfo;

namespace TravelAppWpf.ViewModels
{
    class SignInViewModel : ViewModelBase
    {
        #region Fields And Properties

        string nickOrEmail;
        public string NickOrEmail
        {
            get => nickOrEmail;
            set
            {
                Set(ref nickOrEmail, value);
                ErrorMessage = "";
            }
        }

        string password;
        public string Password
        {
            get => password;
            set
            {
                Set(ref password, value);
                ErrorMessage = "";
            }
        }

        string errorMessage;
        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }

        private string processMessage;

        public string ProcessMessage
        {
            get { return processMessage; }
            set
            {
                processMessage = value;
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }


        #endregion


        #region Dependencies

        IAccountService accountService;
        INavigator navigator;
        private readonly IProcessesInfoService processesInfoService;

        #endregion

        #region Messages

        TripsViewModelMessage tripsViewModelMessage = new TripsViewModelMessage();

        #endregion

        #region Constuctors

        public SignInViewModel(INavigator navigator, IProcessesInfoService processesInfoService, IAccountService accountService)
        {
            this.accountService = accountService;
            this.navigator = navigator;
            this.processesInfoService = processesInfoService;
        }

        #endregion


        #region Commands

        private RelayCommand signInCommand;
        public RelayCommand SignInCommand
        {
            get => signInCommand ?? (signInCommand = new RelayCommand(async () =>
            {


                processesInfoService.ActivateProcess(ProcessEnum.SigningIn, "Signing In");
                UpdateProcessMessage();
                await Task.Run(async () =>
                {

                    var logInResult = await accountService.TryLogInAsync(NickOrEmail, Password);

                    if (logInResult.result)
                    {
                        tripsViewModelMessage.User = logInResult.foundUser;
                        Messenger.Default.Send<TripsViewModelMessage>(tripsViewModelMessage);
                        navigator.NavigateTo<TripsViewModel>();
                    }
                    else
                    {
                        ErrorMessage = "Error occured during signing in\n";
                    }
                });
                processesInfoService.DeactivateProcess(ProcessEnum.SigningIn);
                UpdateProcessMessage();

            }));
        }

        private RelayCommand registerCommand;
        public RelayCommand RegisterCommand
        {
            get => registerCommand ??
                (registerCommand = new RelayCommand(
                    () => navigator.NavigateTo<RegisterViewModel>(),
                    () => string.IsNullOrWhiteSpace(processMessage)
                    ));
        }

        #endregion

        #region Private Functions

        void UpdateProcessMessage()
        {
            try
            {
                ProcessMessage = processesInfoService.GetAllStringValues().Aggregate((i, j) => i + j);
            }
            catch (InvalidOperationException ex)
            {
                ProcessMessage = "";
            }
        }

        #endregion
    }
}
