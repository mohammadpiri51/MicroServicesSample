using Discount.Api.Entities;
using Discount.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.Api.Controllers
{
    [ApiController]
    [Route("api/v1/Discount")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;
        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        [HttpGet("{productName}", Name = "Get")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> Get(string productName)
        {
            var coupon = await _discountRepository.Get(productName);
            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> Create([FromBody] Coupon coupon)
        {
            await _discountRepository.Create(coupon);
            return CreatedAtRoute("Get", new { productName = coupon.ProductName }, coupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> Update([FromBody] Coupon coupon)
        {
            return Ok(await _discountRepository.Update(coupon));
        }

        [HttpDelete("{productName}", Name = "Delete")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> Delete(string productName)
        {
            return Ok(await _discountRepository.Delete(productName));
        }

    }
}
