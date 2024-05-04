using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
        Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
