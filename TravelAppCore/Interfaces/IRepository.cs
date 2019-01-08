using System;
using System.Collections.Generic;
using System.Text;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface IRepository<T>: ISyncRepository<T>, IAsyncRepository<T> where T:BaseEntity
    {
    }
}
