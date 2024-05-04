using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.Interfaces;
using Talabat.Core.Services;
using Talabat.Repository.Repositories;
using Talabat.Service;

namespace Talabat.APIs.Helpers.Extensions
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IPaymentService, PaymentService>();
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(E => E.ErrorMessage).ToArray();
                    var ValidationErrorResponse = new APiValidationErrorResponse()
                    {
                        Errors = errors,
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            return Services;
        }
    }
}
