using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Servicies
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPaymentService paymentService;

        public OrderService
            (IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService
            )
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
            this.paymentService = paymentService;
        }
        public async Task<ProductOrder> CreateOrderAsync(string BuyerEmail, int DeliveryMethodId, string BasketId, ShippingAddress Address)
        {
            // 1. Get The Basket First
            var Basket = await basketRepository.GetBasketAsync(BasketId);
            // 2. Get the Items Inside That Basket
            var Items = new List<OrderItem>();
            foreach (var item in Basket.basketItems)
            {
                var ProductItem = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var ItemOrdered = new ProductItemOrdered(ProductItem.Id,ProductItem.Name,ProductItem.PictureUrl);
                var OrderItem = new OrderItem(ItemOrdered,ProductItem.Price,item.Quantity);
                Items.Add(OrderItem);
            }
           // 3. Get DeliveryMehthod
           var DeliveryMehthod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
           // 4. Calculate SubTotal
           var subtotal = Items.Sum(I => I.Price*I.Quantity);
            // 5. Check If Order Is Exists
            var spec = new OrderWithPaymentIntentSpecifications(Basket.PaymentIntentId);
            var ExistingOrder = await unitOfWork.Repository<ProductOrder>().GetEntityBySpecifications(spec);
            if(ExistingOrder != null)
            {
                unitOfWork.Repository<ProductOrder>().Delete(ExistingOrder);
               await paymentService.CreatOrUpdatPaymentIntent(BasketId);
            }
            // 6. Creat ProductOrder
            var Order = new ProductOrder(BuyerEmail,Address,DeliveryMehthod,Items,subtotal,Basket.PaymentIntentId);
            unitOfWork.Repository<ProductOrder>().Add(Order);
            var result = await unitOfWork.Complete();
            if (result <= 0)
                return null;
            //await basketRepository.DeleteBasketAsync(BasketId);
            return Order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
         => await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

        public async Task<ProductOrder> GetOrderByIdAsync(int Id, string BuyerEmail)
        {
            var OrderSpecification = new OrderWithItemsSpecifications(Id,BuyerEmail);
            return await unitOfWork.Repository<ProductOrder>().GetEntityBySpecifications(OrderSpecification);
        }

        public async Task<IReadOnlyList<ProductOrder>> GetOrdersForUserAsync(string BuyerEmail)
        {
            var OrderSpecification = new OrderWithItemsSpecifications(BuyerEmail);
            return await unitOfWork.Repository<ProductOrder>().ListAsync(OrderSpecification);
        }
    }
}
