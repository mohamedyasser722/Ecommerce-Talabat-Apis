using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Repsoitories;

namespace Talabat.Core.Specifications.Order_Spec
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(string email) 
            : base(O=>O.BuyerEmail == email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDescending(O => O.OrderDate);
        }
        public OrderSpecification(string Email, int orderId) 
            :base(O=>O.BuyerEmail == Email && O.Id== orderId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
