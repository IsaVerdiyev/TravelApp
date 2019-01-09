using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface ISyncRepository<T> where T:BaseEntity
    {
        T GetById(int id, string includePropertyName);
        //T GetSingleBySpec(ISpecification<T> spec);
        IEnumerable<T> ListAll();
        //IEnumerable<T> List(ISpecification<T> spec);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        //int Count(ISpecification<T> spec);
    }
}
