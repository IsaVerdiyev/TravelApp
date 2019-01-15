using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppWpf.Navigation;

namespace TravelAppWpf.ViewModels
{
    class RegisterViewModel : ViewModelBase
    {
        #region Fields And Properties

        string nick;
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
        public string Email {
            get => email;
            set
            {
                Set(ref email, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        string password;
        public string Password {
            get => password;
            set
            {
                Set(ref password, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        string repeatPassword;
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
            get => registerCommand ?? (registerCommand = new RelayCommand(async () =>
            {
                User user = new User
                {
                    Email = Email,
                    NickName = Nick,
                    Password = Password
                };

                bool result = await accountService.TrySignUpAsync(user);
                if (result)
                {
                    MessageBox.Show("User was successfully registered");

                }
                else
                {
                    MessageBox.Show("There is already such user");
                }
            },
            () => !string.IsNullOrWhiteSpace(Nick) &&
                  !string.IsNullOrWhiteSpace(Email) &&
                  !string.IsNullOrWhiteSpace(Password) &&
                  !string.IsNullOrWhiteSpace(RepeatPassword)));
        }

        #endregion
    }
}
