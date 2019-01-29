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

        private string currentProcessesInfo;
        public string CurrentProcessesInfo
        {
            get { return currentProcessesInfo; }
            set
            {
                Set(ref currentProcessesInfo, value);
                RegisterCommand.RaiseCanExecuteChanged();
                SignInCommand.RaiseCanExecuteChanged();
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
        UpdateProcessInfoMessage updateProcessInfoMessage = new UpdateProcessInfoMessage();

        #endregion

        #region Constuctors

        public SignInViewModel(INavigator navigator, IProcessesInfoService processesInfoService, IAccountService accountService)
        {
            this.accountService = accountService;
            this.navigator = navigator;
            this.processesInfoService = processesInfoService;

            Messenger.Default.Register<UpdateProcessInfoMessage>(this, m => UpdateCurrentProcessesInfo());
        }

        #endregion


        #region Commands

        private RelayCommand signInCommand;
        public RelayCommand SignInCommand
        {
            get => signInCommand ?? (signInCommand = new RelayCommand(async () =>
            {

                int processId = processesInfoService.GenerateUniqueId();
                processesInfoService.ActivateProcess(ProcessEnum.SigningIn, processesInfoService.ProcessNames[ProcessEnum.SigningIn], processId);
                try
                {
                    Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
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
                }
                finally
                {
                    processesInfoService.DeactivateProcess(ProcessEnum.SigningIn, processId);
                    Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                }
            },
                () => !processesInfoService.IsProcessActive(ProcessEnum.SigningIn)));
        }

        private RelayCommand registerCommand;
        public RelayCommand RegisterCommand
        {
            get => registerCommand ??
                (registerCommand = new RelayCommand(
                    () => navigator.NavigateTo<RegisterViewModel>(),
                    () => !processesInfoService.IsProcessActive(ProcessEnum.SigningIn)
                    ));
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
