using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Specifications;

namespace TravelAppCore.Interfaces
{
    public interface IAccountService
    {
        (bool result, User foundUser) TryLogIn(string nick, SecureString password);
        Task<(bool result, User foundUser)> TryLogInAsync(string nick, SecureString password);

        bool TrySignUp(User user);
        Task<bool> TrySignUpAsync(User user);

        void DeleteAccount(DeleteByIdSpecification<User> specification);
        Task DeleteAccountAsync(DeleteByIdSpecification<User> specification);
    }
}
