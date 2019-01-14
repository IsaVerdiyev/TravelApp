using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface ISyncRepository<T> where T:BaseEntity
    {
        T GetById(int id);
        T GetSingleBySpec(ISpecification<T> spec);
        IReadOnlyList<T> ListAll();
        IReadOnlyList<T> List(ISpecification<T> spec);
        T Add(T entity);
        void Update(T entity);
        void DeleteBySpec(ISpecification<T> spec);
        int Count(ISpecification<T> spec);
    }
}
