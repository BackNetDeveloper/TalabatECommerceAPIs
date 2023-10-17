using APIsMainProject.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIsMainProject.Controllers
{
   
    public class BasketController : BaseController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketController(IBasketRepository basketRepository ,IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }
        [HttpGet("GetBasket")]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var Basket = await basketRepository.GetBasketAsync(id);
            return Ok(Basket ?? new CustomerBasket(id));
        }
        [HttpPost("UpdateBasket")]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto customerBasketDto)
        {
            var Basket = mapper.Map<CustomerBasket>(customerBasketDto);
            var updatedBasket = await basketRepository.UpdateBasketAsync(Basket);
            return Ok(updatedBasket);
        }
        [HttpDelete("DeleteBasket")]
        public  Task DeleteBasketById(string id)
          => basketRepository.DeleteBasketAsync(id);
        
    }
}
