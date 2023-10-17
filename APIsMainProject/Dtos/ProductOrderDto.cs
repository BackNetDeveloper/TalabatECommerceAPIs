

namespace APIsMainProject.Dtos
{
    public class ProductOrderDto
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public ShippingAddressDto Address { get; set; }
        public string? PaymentIntentId { get; set; }

    }
}
