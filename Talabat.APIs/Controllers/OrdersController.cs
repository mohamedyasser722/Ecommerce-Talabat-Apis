using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Services;
using Order = Talabat.Core.Entities.Order_Aggregation.Order;
namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var address = _mapper.Map<Address>(orderDto.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);
            if (order is null) return BadRequest(new ApiErrorResponse(400));
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);
            var mappedOrders = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
            return Ok(mappedOrders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var buyerEMail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForUserAsync(id, buyerEMail);
            if (order is null) 
                return NotFound(new ApiErrorResponse(404));

            var mappedOrder = _mapper.Map<OrderToReturnDto>(order);
            return Ok(mappedOrder);
        }

        [HttpGet("deliverymethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }
    }
}
