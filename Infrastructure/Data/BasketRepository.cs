using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }
        public async Task<CustomerBasket> GetBasketAsync(string BasketId)
        {
            var data = await database.StringGetAsync(BasketId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }
      
        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket)
        {
            var CreatedData = await database.StringSetAsync(customerBasket.Id,
                                                     JsonSerializer.Serialize(customerBasket),
                                                     TimeSpan.FromDays(30));
            if (!CreatedData)
                return null;
            return await GetBasketAsync(customerBasket.Id);
        }
        public async Task DeleteBasketAsync(string BasketId)
        {
            await database.KeyDeleteAsync(BasketId);
        }
    }
}
