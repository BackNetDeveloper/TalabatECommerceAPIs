using APIsMainProject.Helper;
using APIsMainProject.ResponseModule;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Servicies;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace APIsMainProject.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection GetApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBasketRepository,BasketRepository>();
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<IOrderService,OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IResponseCashService, ResponseCashService>();
            services.AddAutoMapper(typeof(MappingProfiles));


            services.Configure<ApiBehaviorOptions>(option => 
            {
                option.InvalidModelStateResponseFactory = ActionContext =>
                {
                    var Errors = ActionContext.ModelState.Where(E => E.Value.Errors.Count > 0)
                                                         .SelectMany(E => E.Value.Errors)
                                                         .Select(E=>E.ErrorMessage).ToArray();
                    var Responces = new ApiValidationErrorResponse
                    {
                        Errors = Errors
                    };
                    return new BadRequestObjectResult(Responces);
                };
            });
            return services;
        }
    }
}
