using System;
using System.Collections.Generic;
using System.Text;
using TravelAppCore.Entities;

namespace TravelAppCore.Specifications
{
    public class DeleteByIdSpecification<T>: BaseSpecification<T> where T: BaseEntity
    {
        public DeleteByIdSpecification(int id): base(item => item.Id == id)
        {

        }
    }
}
