using Basket.api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.api.Repositoreis
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<ShoppingCart> Get(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
                return new ShoppingCart(userName);
            return JsonConvert.DeserializeObject<ShoppingCart>(basket) ?? new ShoppingCart(userName);
        }

        public async Task<ShoppingCart> Update(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return await Get(basket.UserName);
        }

        public async Task Delete(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }
    }
}
