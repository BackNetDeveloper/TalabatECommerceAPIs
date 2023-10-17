
using Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Servicies
{
    public class ResponseCashService : IResponseCashService
    {
        private readonly IDatabase database;
        public ResponseCashService(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }
        public async Task CashResponseAsync(string CashKey, object Response, TimeSpan TimeToLive)
        {
            if (Response is null)
                return;
            var serializedresponse = JsonSerializer.Serialize(Response);
            await database.StringSetAsync(CashKey, serializedresponse,TimeToLive);
        }

        public async Task<string> GetCashedResponse(string CashKey)
        {
            var CashedResponse = await database.StringGetAsync(CashKey);
            if (CashedResponse.IsNullOrEmpty)
                return null;
            return CashedResponse;
        }
    }
}
