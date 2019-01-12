using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface IAccountService
    {
        (bool result, User foundUser) TryLogIn(string nick, string password);
        Task<(bool result, User foundUser)> TryLogInAsync(string nick, string password);

        bool TrySignUp(User user);
        Task<bool> TrySignUpAsync(User user);

        void DeleteAccount(User user);
        Task DeleteAccountAsync(User user);
    }
}
