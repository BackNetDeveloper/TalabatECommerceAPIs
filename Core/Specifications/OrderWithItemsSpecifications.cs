using Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class OrderWithItemsSpecifications : BaseSpecification<ProductOrder>
    {
        // Return List Of Orders
        public OrderWithItemsSpecifications(string Email) : base(order=>order.BuyerEmail == Email)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDesending(order => order.OrderDate);
        }
        // Return One Orders
        public OrderWithItemsSpecifications(int id ,string Email) :
            base(order =>order.Id == id && order.BuyerEmail == Email)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
        }
    }
}
