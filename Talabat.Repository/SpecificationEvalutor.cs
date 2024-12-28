using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Enitities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static  class SpecificationEvalutor<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery , ISpecifications<T> spec )
        {
            var Query = InputQuery; //_dbContext.Set<T>()

            if(spec.Criteria is not null)
            {
                Query = Query.Where(spec.Criteria); //_dbContext.Set<T>().where(P =>P,Id==id)

            }

            if(spec.OrderBy is not null)
                Query = Query.OrderBy(spec.OrderBy);

            if (spec.OrderByDescending is not null)
                Query = Query.OrderBy(spec.OrderByDescending);

            if (spec.IsPaginationEnabled)
            {
               Query =  Query.Skip(spec.Skip).Take(spec.Take);
            }
            //P => P.ProductBrand , P => P.ProductType
            Query = spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));
            //_dbContext.Set<T>().where(P =>P,Id==id).Include(P => P.ProductBrand)
            //_dbContext.Set<T>().where(P =>P,Id==id).Include(P => P.ProductBrand).Include(P => P.ProductType)

            
            return Query;

        }
    }
}
