using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Enitities.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Spec
{
    public class OrderWithPaymnetIntentIdSpec : BaseSpecifications<Order>

    {
        public OrderWithPaymnetIntentIdSpec(string PaymentIntentId) : base(Order => Order.PaymentIntentId== PaymentIntentId)
        {
            
        }
    }
}
