﻿using Core.Entities.OrderAggregate;

namespace APIsMainProject.Dtos
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public ShippingAddressDto ShipedToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        public string OrderStatus { get; set; } 
        public decimal Total { get; set; }
        public string? PaymentIntentId { get; set; }

    }
}
