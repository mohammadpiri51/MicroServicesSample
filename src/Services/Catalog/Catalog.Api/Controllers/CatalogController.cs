using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/v1/catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository ProductRepository;
        private readonly ILogger<CatalogController> Logger;
        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            ProductRepository = productRepository;
            Logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await ProductRepository.Get();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetById(string id)
        {
            var product = await ProductRepository.Get(id);
            if (product == null)
            {
                Logger.LogError($"Product with id: {id} not found");
                return NotFound();
            }
            return Ok(product);
        }

        [Route("GetByCategory/{category}", Name = "GetByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetByCategory(string category)
        {
            var product = await ProductRepository.GetByCategory(category);
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            await ProductRepository.Create(product);
            return CreatedAtRoute("GetById", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] Product product)
        {
            return Ok(await ProductRepository.Update(product));
        }

        [HttpDelete("{id:length(24)}",Name = "Delete")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await ProductRepository.Delete(id));
        }
    }
}
