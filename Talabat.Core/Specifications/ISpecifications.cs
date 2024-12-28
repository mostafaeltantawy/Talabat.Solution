using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Enitities;

namespace Talabat.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        // Signature For property for where condition [Where(P=>P.Id)]

        public Expression<Func<T ,bool>> Criteria{ get; set; }


        // Signature For property forlist of  includes condition [Incliude(Product => Product.ProductBrand]

        public List<Expression<Func<T,object>>> Includes { get; set; }

        // Property For Order By 
        public Expression<Func<T,object>> OrderBy { get; set; }

        // Property for order by descending
        public Expression<Func<T,object>> OrderByDescending { get; set; }

        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }

    }
}
