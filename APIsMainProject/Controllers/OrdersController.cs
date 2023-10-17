using APIsMainProject.Dtos;
using APIsMainProject.Extentions;
using APIsMainProject.ResponseModule;
using AutoMapper;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIsMainProject.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<ProductOrder>> CreateOrder(ProductOrderDto productOrderDto)
        {
            var Email = HttpContext.User.RetrieveEmailFromPrincipal();
            var address = mapper.Map<ShippingAddress>(productOrderDto.Address);
            var order = await orderService.CreateOrderAsync(Email, productOrderDto.DeliveryMethodId,
                                                             productOrderDto.BasketId, address);
            if (order is null)
                return BadRequest(new ApiResponse(400, "Problem When Creating Order "));
            return Ok(order);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderDetailsForUserById(int Id)
        {
            var Email = HttpContext.User.RetrieveEmailFromPrincipal();
            var order = await orderService.GetOrderByIdAsync(Id, Email);
            if (order is null)
                return BadRequest(new ApiResponse(404, "Order Dose Not Exists"));
            return Ok(mapper.Map<OrderDetailsDto>(order));
        }
        [HttpGet("GetAllOrdersForUser")]
        public async Task<ActionResult<IReadOnlyList<OrderDetailsDto>>> GetOrdersForUser()
        {
            var Email = HttpContext.User.RetrieveEmailFromPrincipal();
            var orders = await orderService.GetOrdersForUserAsync(Email);

            return Ok(mapper.Map<IReadOnlyList<OrderDetailsDto>>(orders));
        }
        [HttpGet("GetDelivreyMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethodsForUser()
            => Ok(await orderService.GetDeliveryMethodsAsync());
    }
}
