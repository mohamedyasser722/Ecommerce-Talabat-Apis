using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;

namespace Talabat.APIs.Controllers
{
    public class BasketsController : BaseApiController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketsController(IBasketRepository basketRepository, IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string id)
        {
            var basket = await basketRepository.GetBasketAsync(id);
            ///if (basket == null)
            ///    return new CustomerBasket(id);
            ///return basket;
            return basket is null ? new CustomerBasket(id) : basket;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasket(CustomerBasketDto basket)
        {
            var MappedBasket = mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var createdOrUpdatedBasket = await basketRepository.UpdateBasketAsync(MappedBasket);
            if (createdOrUpdatedBasket is null)
                return BadRequest(new ApiErrorResponse(400));
            return Ok(createdOrUpdatedBasket);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await basketRepository.DeleteBasketAsync(id);
        }
    }
}
