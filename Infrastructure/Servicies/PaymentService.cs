using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Core.Entities.Product;

namespace Infrastructure.Servicies
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketRepository basketRepository;
        private readonly IConfiguration config;

        public PaymentService(
                              IUnitOfWork unitOfWork,
                              IBasketRepository basketRepository,
                              IConfiguration config
                             )
        {
            this.unitOfWork = unitOfWork;
            this.basketRepository = basketRepository;
            this.config = config;
        }
        public async Task<CustomerBasket> CreatOrUpdatPaymentIntent(string basketid)
        {
            StripeConfiguration.ApiKey = config["StripeSettings:Secret key"]; // يعتبر دا التوكن بتاعى 
            var basket = await basketRepository.GetBasketAsync(basketid);
            if (basket == null)
                return null;
            var ShippingPrice = 0m;
            if (basket.DelivaryMethodId.HasValue)
            {
                var delivarymethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DelivaryMethodId.Value);
                 ShippingPrice = delivarymethod.Price;
            }
            foreach (var item in basket.basketItems)
            {
                var ProductItem = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (item.Price != ProductItem.Price)
                    item.Price = ProductItem.Price;
            }
            var Service = new PaymentIntentService();
            PaymentIntent intent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount =(long) (basket.basketItems.Sum(item => item.Quantity * (item.Price*100))+(ShippingPrice*100)),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> {"card"},
                };
                intent = await Service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(basket.basketItems
                                          .Sum(item => item.Quantity * (item.Price * 100)) + (ShippingPrice * 100))
                };
                await Service.UpdateAsync(basket.PaymentIntentId, options);
            }
            await basketRepository.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<ProductOrder> UpdateOrderPaymentFaild(string PaymentIntentId)
        {
            var spec = new OrderWithItemsSpecifications(PaymentIntentId);
            var Order = await unitOfWork.Repository<ProductOrder>().GetEntityBySpecifications(spec);
            if (Order is null)
                return null;
            Order.OrderStatus = OrderStatus.PaymentFaild;
             unitOfWork.Repository<ProductOrder>().Update(Order);
            await unitOfWork.Complete();
            return Order;
        }

        public async Task<ProductOrder> UpdateOrderPaymentSucceeded(string PaymentIntentId)
        {
            var spec = new OrderWithItemsSpecifications(PaymentIntentId);
            var Order = await unitOfWork.Repository<ProductOrder>().GetEntityBySpecifications(spec);
            if (Order is null)
                return null;
            Order.OrderStatus = OrderStatus.paymentReceived;
            unitOfWork.Repository<ProductOrder>().Update(Order);
            await unitOfWork.Complete();
            return Order;
        }
    }
}
