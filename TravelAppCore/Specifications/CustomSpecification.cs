using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TravelAppCore.Entities;

namespace TravelAppCore.Specifications
{
    public class CustomSpecification<T>: BaseSpecification<T> where T:BaseEntity
    {
        public CustomSpecification(Expression<Func<T, bool>> criteria, IEnumerable<Expression<Func<T, object>>> Includes = null, IEnumerable<string> IncludeStrings = null): base(criteria)
        {
            if (Includes != null)
            {
                this.Includes.AddRange(Includes);
            }
            if (IncludeStrings != null)
            {
                this.IncludeStrings.AddRange(IncludeStrings);
            }
        }
    }
}
