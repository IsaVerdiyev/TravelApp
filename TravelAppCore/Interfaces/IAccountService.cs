using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface IAccountService
    {
        bool TryLogIn(string nick, string password, out User user);
        Task<bool> TryLogInAsync(string nick, string password, out User user);

        bool TrySignUp(ref User user);
        Task<bool> TrySignUpAsync(ref User user);
    }
}
