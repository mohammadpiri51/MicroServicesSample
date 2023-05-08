using Dapper;
using Discount.Api.Entities;
using Npgsql;

namespace Discount.Api.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Coupon> Get(string productName)
        {
            NpgsqlConnection connection = GetConnection();
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @productName", new { ProductName = productName });
            if (coupon == null)
                return new Coupon() { ProductName = "No Coupon" };
            return coupon;
        }

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }

        public async Task<bool> Create(Coupon coupon)
        {
            NpgsqlConnection connection = GetConnection();
            var affected = await connection.ExecuteAsync(
                "INSERT INTO Coupon (ProductName,Description,Amount) VALUES (@ProductName,@Description,@Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
            if (affected == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Update(Coupon coupon)
        {
            NpgsqlConnection connection = GetConnection();
            var affected = await connection.ExecuteAsync(
                "UPDATE Coupon  SET ProductName = @ProductName,Description = @Description,Amount = @Amount Where Id = @Id",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });
            if (affected == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Delete(string productName)
        {
            NpgsqlConnection connection = GetConnection();
            var affected = await connection.ExecuteAsync(
                "Delete FROM Coupon Where ProductName = @ProductName",
                new { ProductName = productName });
            if (affected == 0)
            {
                return false;
            }
            return true;
        }


    }
}
