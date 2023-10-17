using Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<ProductOrder> CreateOrderAsync(string BuyerEmail , int DeliveryMethodId,string BasketId,ShippingAddress Address);
        Task<IReadOnlyList<ProductOrder>> GetOrdersForUserAsync(string BuyerEmail);
        Task<ProductOrder> GetOrderByIdAsync(int Id, string BuyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();

    }
}
