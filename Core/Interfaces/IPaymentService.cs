using Core.Entities;
using Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreatOrUpdatPaymentIntent(string basketid);
        Task<ProductOrder> UpdateOrderPaymentSucceeded(string PaymentIntentId);
        Task<ProductOrder> UpdateOrderPaymentFaild(string PaymentIntentId);
       
    }
}
