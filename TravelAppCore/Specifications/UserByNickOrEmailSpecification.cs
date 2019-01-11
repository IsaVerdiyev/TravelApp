using System;
using System.Collections.Generic;
using System.Text;
using TravelAppCore.Entities;

namespace TravelAppCore.Specifications
{
    public class UserByNickOrEmailSpecification: BaseSpecification<User>
    {
        public UserByNickOrEmailSpecification(string nickOrEmail): base(u => string.Compare(u.NickName, nickOrEmail) == 0 || string.Compare(u.Email, nickOrEmail) == 0)
        {

        }
    }
}
