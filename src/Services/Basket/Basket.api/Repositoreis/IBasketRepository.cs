using Basket.api.Entities;

namespace Basket.api.Repositoreis
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> Get(string userName);
        Task<ShoppingCart> Update(ShoppingCart basket);
        Task Delete(string userName);
    }
}
