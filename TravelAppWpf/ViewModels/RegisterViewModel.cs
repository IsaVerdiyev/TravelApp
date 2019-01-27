using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    class RegisterViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Fields And Properties

        

        string nick;
        [Required(ErrorMessage = "Nick field is required")]
        public string Nick
        {
            get => nick;
            set
            {
                Set(ref nick, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        string email;
        [Required(ErrorMessage = "Email field is required")]
        [EmailAddress]
        public string Email
        {
            get => email;
            set
            {
                Set(ref email, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        string password;
        [Required(ErrorMessage = "Password field is required")]
        public string Password
        {
            get => password;
            set
            {
                Set(ref password, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        string repeatPassword;

        [Required(ErrorMessage = "Repeat password to confirm it")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string RepeatPassword
        {
            get => repeatPassword;
            set
            {
                Set(ref repeatPassword, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }


        private string currentProcessesInfo;
        public string CurrentProcessesInfo
        {
            get { return currentProcessesInfo; }
            set
            {
                Set(ref currentProcessesInfo, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion


        #region Dependencies

        INavigator navigator;
        private readonly IProcessesInfoService processesInfoService;
        IAccountService accountService;

        #endregion

        #region Messages

        TripsViewModelMessage tripsViewModelMessage = new TripsViewModelMessage();
        UpdateProcessInfoMessage updateProcessInfoMessage = new UpdateProcessInfoMessage();
        #endregion

        #region Constructors

        public RegisterViewModel(INavigator navigator, IProcessesInfoService processesInfoService, IAccountService accountService)
        {
            this.navigator = navigator;
            this.processesInfoService = processesInfoService;
            this.accountService = accountService;

            Messenger.Default.Register<UpdateProcessInfoMessage>(this, m => UpdateCurrentProcessInfo());
        }

        #endregion


        #region Commands

        private RelayCommand registerCommand;
        public RelayCommand RegisterCommand
        {
            get => registerCommand ?? (registerCommand = new RelayCommand(async () =>
            {
                int processId = processesInfoService.GenerateUniqueId();
                processesInfoService.ActivateProcess(ProcessEnum.SigningUp, processesInfoService.ProcessNames[ProcessEnum.SigningUp], processId);
                Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                await Task.Run(async () =>
                {
                    User user = new User
                    {
                        Email = Email,
                        NickName = Nick,
                        Password = Password
                    };

                    var signUpResult = await accountService.TrySignUpAsync(user);
                    if (signUpResult)
                    {
                        MessageBox.Show("User was successfully registered");
                        tripsViewModelMessage.User = user;
                        Messenger.Default.Send<TripsViewModelMessage>(tripsViewModelMessage);
                        navigator.NavigateTo<TripsViewModel>();
                    }
                    else
                    {
                        MessageBox.Show("There is already such a user");
                    }
                });
                processesInfoService.DeactivateProcess(ProcessEnum.SigningUp, processId);
                Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
            },
            () => !string.IsNullOrWhiteSpace(Nick) &&
                  !string.IsNullOrWhiteSpace(Email) &&
                  !string.IsNullOrWhiteSpace(Password) &&
                  !string.IsNullOrWhiteSpace(RepeatPassword) &&
                  string.IsNullOrWhiteSpace(CurrentProcessesInfo) &&
                  Password.Equals(RepeatPassword)));

        }

        private RelayCommand returnBackCommand;
        public RelayCommand ReturnBackCommand
        {
            get => returnBackCommand ?? (returnBackCommand = new RelayCommand(() => navigator.NavigateTo<SignInViewModel>()));
        }

        #endregion

        #region Validation

        public string Error => throw new NotImplementedException();

        public string this[string columnName] => this.Validate(columnName);

        #endregion

        #region Private Functions

        void UpdateCurrentProcessInfo()
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
