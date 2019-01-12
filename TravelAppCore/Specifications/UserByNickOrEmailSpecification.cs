using System;
using System.Collections.Generic;
using System.Text;
using TravelAppCore.Entities;

namespace TravelAppCore.Specifications
{
    public class UserByNickOrEmailSpecification: BaseSpecification<User>
    {
        public UserByNickOrEmailSpecification(string nickOrEmail): base(u => u.NickName.Equals(nickOrEmail) || u.Email.Equals(nickOrEmail))
        {

        }

        public UserByNickOrEmailSpecification(string nick, string email): base(u => u.NickName.Equals(nick) || u.Email.Equals(email))
        {

        }
    }
}
