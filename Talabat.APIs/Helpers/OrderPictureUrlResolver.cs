using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.APIs.Helpers
{
    public class OrderPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration configuration;

        public OrderPictureUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{configuration.GetSection("ApiBaseUrl").Value}{source.Product.PictureUrl}";

            return string.Empty;
        }
    }
}
