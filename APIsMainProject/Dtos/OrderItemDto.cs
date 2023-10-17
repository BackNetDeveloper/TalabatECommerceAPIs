using Core.Entities.OrderAggregate;

namespace APIsMainProject.Dtos
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductItemName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}