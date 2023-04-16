using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;


namespace Catalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> Get()
        {
            return await _context.Products.Find(prop => true).ToListAsync();
        }

        public async Task<Product> Get(string id)
        {
            return await _context.Products.Find(prop => prop.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategory(string category)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Name, name);
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }
        public async Task<bool> Update(Product product)
        {
            var updateResult = await _context.Products
                                    .ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            var DeleteResult = await _context.Products
                                   .DeleteOneAsync(filter);
            return DeleteResult.IsAcknowledged && DeleteResult.DeletedCount > 0;
        }

    }
}
