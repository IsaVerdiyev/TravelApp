using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;

namespace TravelAppInfrastructure.Data
{
    public class SpecificationEvaluator<T> where T:BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            if(specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}
