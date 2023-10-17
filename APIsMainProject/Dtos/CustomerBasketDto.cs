using Core.Entities;

namespace APIsMainProject.Dtos
{
    public class CustomerBasketDto
    {
        public string Id { get; set; }
        public int? DelivaryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
        public List<BasketItemDto> basketItems { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }

   
}
