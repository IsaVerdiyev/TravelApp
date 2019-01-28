using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using Z.EntityFramework.Plus;

namespace TravelAppInfrastructure.Data
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {

        TripDb tripDb;

        public EfRepository(TripDb tripDb)
        {
            this.tripDb = tripDb;
        }

        public T Add(T entity)
        {
            T added = tripDb.Set<T>().Add(entity);
            tripDb.SaveChanges();
            return added;
        }

        public async Task<T> AddAsync(T entity)
        {
            T added = tripDb.Set<T>().Add(entity);
            await tripDb.SaveChangesAsync();
            return added;
        }

        public int Count(ISpecification<T> spec)
        {
            return ApplySpecification(spec).Count();
        }

        public Task<int> CountAsync(ISpecification<T> spec)
        {
            return ApplySpecification(spec).CountAsync();
        }

        public void DeleteBySpec(ISpecification<T> spec)
        {
          
            ApplySpecification(spec).DeleteFromQuery();
        }

        public async Task DeleteBySpecAsync(ISpecification<T> spec)
        {
            await ApplySpecification(spec).DeleteFromQueryAsync();
        }

        public T GetById(int id)
        {
            T found = tripDb.Set<T>().Find(id);
            return found;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            T found =  await tripDb.Set<T>().FindAsync(id);
            return found;
        }

        public T GetSingleBySpec(ISpecification<T> spec)
        {
            return ApplySpecification(spec).FirstOrDefault();
        }

        public async Task<T> GetSingleBySpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public IReadOnlyList<T> List(ISpecification<T> spec)
        {
            return ApplySpecification(spec).ToList();
        }

        public IReadOnlyList<T> ListAll()
        {
            return tripDb.Set<T>().ToList();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await tripDb.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public void Update(T entity)
        {
            tripDb.Entry<T>(entity).State = EntityState.Modified;
            tripDb.SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            tripDb.Entry<T>(entity).State = EntityState.Modified;
            await tripDb.SaveChangesAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(tripDb.Set<T>().AsQueryable(), specification);
        }

    }
}
