using System;
using System.Collections.Generic;
using System.Security;
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

        string salt = "alsdjf;ahjg;ha;sdnv;khasdhfa";

        

        public AccountService(IRepository<User> userRepos)
        {
            this.userRepos = userRepos;
           
        }

        public void DeleteAccount(DeleteByIdSpecification<User> specification)
        {
            
            userRepos.DeleteBySpec(specification);
        }

        public async Task DeleteAccountAsync(DeleteByIdSpecification<User> specification)
        {
            
            await userRepos.DeleteBySpecAsync(specification);
        }

        public (bool, User) TryLogIn(string nick, SecureString password)
        {
            

            User foundUser = userRepos.GetSingleBySpec(new UserByNickOrEmailSpecification(nick));

            return GetSigningInResult(nick,password, foundUser);
            
        }

        public async Task<(bool, User)> TryLogInAsync(string nickOrEmail, SecureString password)
        {
           

            User foundUser = await userRepos.GetSingleBySpecAsync(new UserByNickOrEmailSpecification(nickOrEmail));

            return GetSigningInResult(nickOrEmail, password, foundUser);
           
        }

        public bool TrySignUp(User user)
        {
            User foundUser = userRepos.GetSingleBySpec(new UserByNickOrEmailSpecification(user.NickName, user.Email));
            if(foundUser != null)
            {
                return false;
            }

            else
            {
                user.Password = Encrypt(user.Password);
                userRepos.Add(user);
                return true;
            }
        }

        public async Task<bool> TrySignUpAsync(User user)
        {
            User foundUser = await userRepos.GetSingleBySpecAsync(new UserByNickOrEmailSpecification(user.NickName, user.Email));
            if (foundUser != null)
            {
                return false;
            }

            else
            {
                user.Password = Encrypt(user.Password);
                await userRepos.AddAsync(user);
                return true;
            }
        }


        private string Encrypt(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
                bytes = sha.ComputeHash(bytes);
                return Encoding.Default.GetString(bytes);
            }
        }

        


        private (bool, User) GetSigningInResult(string nickOrEmail, SecureString password, User foundUser)
        {
            if (foundUser == null)
            {
                return (false, foundUser);
            }
            if (!foundUser.NickName.Equals(nickOrEmail) && !foundUser.Email.Equals(nickOrEmail))
            {
                foundUser = null;
                return (false, foundUser);
            }
            string realPassword = new System.Net.NetworkCredential(string.Empty, password).Password;
            if (foundUser.Password == Encrypt(realPassword))
            {
                
                return (true, foundUser);
            }
            else
            {
                foundUser = null;
                return (false, foundUser);
            }
        }
    }
}
