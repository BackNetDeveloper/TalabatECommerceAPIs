using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public class ProductOrder:BaseEntity
    {
        public ProductOrder()
        {

        }
        public ProductOrder(string buyerEmail,
               ShippingAddress shipedToAddress,
               DeliveryMethod deliveryMethod, 
               IReadOnlyList<OrderItem> orderItems,
               decimal subTotal,
               string paymentintentid)
        {
               BuyerEmail = buyerEmail;
               ShipedToAddress = shipedToAddress;
               DeliveryMethod = deliveryMethod;
               OrderItems = orderItems;
               SubTotal = subTotal;
               PaymentIntentId = paymentintentid;
        }

        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public ShippingAddress ShipedToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public string? PaymentIntentId { get; set; }
        public decimal GetTotal() => SubTotal + DeliveryMethod.Price;
    }
}
