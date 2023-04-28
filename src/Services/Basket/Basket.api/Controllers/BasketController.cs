using Basket.api.Entities;
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

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
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
            return Ok(await _basketRepository.Update(basket));
        }

        [HttpDelete("{userName}",Name = "Delete")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(string userName)
        {
            await _basketRepository.Delete(userName);
            return Ok();
        }
    }
}
