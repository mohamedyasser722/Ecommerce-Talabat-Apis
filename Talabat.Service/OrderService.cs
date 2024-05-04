using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Interfaces;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Spec;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //1. Get Basket from BasketRepo
            var basket = await _basketRepository.GetBasketAsync(basketId);

            //2. Get Selected Items at basket from Product Repository
            var orderItems = new List<OrderItem>();
            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrdered = new ProductOrderItem(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }
            }

            //3. Calculate SubTotal 
            var subTotal = orderItems.Sum(OI => OI.Price * OI.Quantity);

            //4. Get Delivery Method from Delivery Method Repository
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            //Check Order Exists with the same PaymentIntentId
            var spec = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var existOrder = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            if (existOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(existOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }
            
            //5. Create Order
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal, basket.PaymentIntentId);

            //6. Add Order Locally
            await _unitOfWork.Repository<Order>().Add(order); // Added Locally

            //7. Save Order To DataBase
            var result = await _unitOfWork.Complete();

            if (result <= 0)
                return null;

            return order;
        }
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }

        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var Spec = new OrderSpecification(buyerEmail, orderId);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(Spec);
            return (order);
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveryMethods;
        }
    }
}
