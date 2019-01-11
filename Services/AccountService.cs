using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppCore.Specifications;

namespace Services
{
    public class AccountService : IAccountService
    {
        IRepository<User> userRepos;

        string salt = "sdflkj;adsf";

        public AccountService(IRepository<User> userRepos)
        {
            this.userRepos = userRepos;
        }

        public bool TryLogIn(string nick, string password, out User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryLogInAsync(string nick, string password, out User user)
        {
            throw new NotImplementedException();
        }

        public bool TrySignUp(ref User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TrySignUpAsync(ref User user)
        {
            throw new NotImplementedException();
        }
    }
}
