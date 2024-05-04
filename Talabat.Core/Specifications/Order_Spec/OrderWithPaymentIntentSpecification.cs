using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Repsoitories;

namespace Talabat.Core.Specifications.Order_Spec
{
    public class OrderWithPaymentIntentSpecification : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecification(string intentId) : base(O => O.PaymentIntentId == intentId)
        {

        }
    }
}
