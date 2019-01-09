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

        public void Delete(T entity)
        {
            tripDb.Set<T>().Remove(entity);
            tripDb.SaveChanges();
        }

        public async Task DeleteAsync(T entity)
        {
            tripDb.Set<T>().Remove(entity);
            await tripDb.SaveChangesAsync();
        }

        public T GetById(int id, string inlcudePropertyName = null)
        {
            if (inlcudePropertyName != null)
            {
                return tripDb.Set<T>().Include(inlcudePropertyName).SingleOrDefault(item => item.Id == id);
            }
            else
            {
                return tripDb.Set<T>().Find(id);
            }
        }

        public async Task<T> GetByIdAsync(int id, string includePropertyName = null)
        {
            if (includePropertyName != null)
            {
                return await tripDb.Set<T>().Include(includePropertyName).SingleOrDefaultAsync(item => item.Id == id);
            }
            else
            {
                return await tripDb.Set<T>().FindAsync(id);
            }
        }

        public IEnumerable<T> ListAll()
        {
            return tripDb.Set<T>().AsNoTracking().ToList();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await tripDb.Set<T>().AsNoTracking().ToListAsync();
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

        
    }
}
