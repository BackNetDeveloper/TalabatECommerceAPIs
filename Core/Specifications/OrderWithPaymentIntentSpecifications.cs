using Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class OrderWithPaymentIntentSpecifications : BaseSpecification<ProductOrder>
    {
        public OrderWithPaymentIntentSpecifications(string PaymentIntentId) 
            : base(Order => Order.PaymentIntentId == PaymentIntentId)
        {
        }
    }
}
