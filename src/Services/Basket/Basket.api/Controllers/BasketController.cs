using Basket.api.Entities;
using Basket.api.GrpcServices;
using Basket.api.Repositoreis;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.api.Controllers
{
    [ApiController]
    [Route("api/v1/Basket")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
        {
            _basketRepository = basketRepository;
            _discountGrpcService = discountGrpcService;

        }

        [HttpGet("{userName}", Name = "Get")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> Get(string userName)
        {
            var basket = await _basketRepository.Get(userName);
            return Ok(basket);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> Update([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.ShoppingCartItems)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
            return Ok(await _basketRepository.Update(basket));
        }

        [HttpDelete("{userName}", Name = "Delete")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(string userName)
        {
            await _basketRepository.Delete(userName);
            return Ok();
        }
    }
}
