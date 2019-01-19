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

namespace TravelAppWpf.ViewModels
{
    class RegisterViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Fields And Properties

        string nick;
        [Required (ErrorMessage ="Nick field is required")]
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
        [Required (ErrorMessage = "Email field is required")]
        [EmailAddress]
        public string Email {
            get => email;
            set
            {
                Set(ref email, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        string password;
        [Required (ErrorMessage ="Password field is required")]
        public string Password {
            get => password;
            set
            {
                Set(ref password, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        string repeatPassword;

        [Required (ErrorMessage = "Repeat password to confirm it")]
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


        #endregion


        #region Dependencies

        INavigator navigator;
        IAccountService accountService;

        #endregion

        #region Messages

        TripsViewModelMessage tripsViewModelMessage = new TripsViewModelMessage();

        #endregion

        #region Constutors

        public RegisterViewModel(INavigator navigator, IAccountService accountService)
        {
            this.navigator = navigator;
            this.accountService = accountService;
        }

        #endregion


        #region Commands

        private RelayCommand registerCommand;
        public RelayCommand RegisterCommand
        {
            get => registerCommand ?? (registerCommand = new RelayCommand(() =>
            {
                User user = new User
                {
                    Email = Email,
                    NickName = Nick,
                    Password = Password
                };

                var signUpTask = Task.Run<bool>(async () => await accountService.TrySignUpAsync(user));

                signUpTask.ContinueWith(t => {
                    if (t.Result)
                    {
                        MessageBox.Show("User was successfully registered");
                        tripsViewModelMessage.User = user;
                        Messenger.Default.Send<TripsViewModelMessage>(tripsViewModelMessage);
                        navigator.NavigateTo<TripsViewModel>();
                    }
                    else
                    {
                        MessageBox.Show("There is already such user");
                    }

                }, TaskScheduler.Current);
            },
            () => !string.IsNullOrWhiteSpace(Nick) &&
                  !string.IsNullOrWhiteSpace(Email) &&
                  !string.IsNullOrWhiteSpace(Password) &&
                  !string.IsNullOrWhiteSpace(RepeatPassword) &&
                  Password.Equals(RepeatPassword)));
                
        }

        private RelayCommand returnBackCommand;
        public RelayCommand ReturnBackCommand
        {
            get  => returnBackCommand ?? (returnBackCommand  = new RelayCommand(() => navigator.NavigateTo<SignInViewModel>()))  ;
        }

        #endregion

        #region Validation

        public string Error => throw new NotImplementedException();

        public string this[string columnName] => this.Validate(columnName);

        #endregion
    }
}
