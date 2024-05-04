using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregation;


namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(PD => PD.Brand, O => O.MapFrom(P => P.Brand.Name))
                .ForMember(PD => PD.ProductCategory, O => O.MapFrom(P => P.ProductCategory.Name))
                .ForMember(PD => PD.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto,Talabat.Core.Entities.Order_Aggregation.Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(D => D.DeliveryMethod, o => o.MapFrom(M => M.DeliveryMethod.ShortName))
                .ForMember(D => D.DeliveryMethodCost, o => o.MapFrom(M => M.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(D => D.ProductId, O => O.MapFrom(M => M.Product.ProductId))
                .ForMember(D => D.ProductName, O => O.MapFrom(M => M.Product.ProductName))
                .ForMember(D => D.PictureUrl, O => O.MapFrom(M => M.Product.PictureUrl))
                .ForMember(D => D.PictureUrl, O => O.MapFrom<OrderPictureUrlResolver>());
                
        }
    }
}
