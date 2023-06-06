using Npgsql;

namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDataBase<TContext>(this IHost host, int? retry = 0)
        {
            var retryForAvailability = retry ?? 0;

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var logger = serviceProvider.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrationg Posgresql DataBase");

                    var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    var command = new NpgsqlCommand
                    {
                        Connection = connection,
                    };

                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon
                                                    (
                                                        Id SERIAL PRIMARY KEY,
                                                        ProductName VARCHAR(24) NOT NULL,
                                                        Description TEXT,
                                                        Amount INT
                                                    )";
                    command.ExecuteNonQuery();

                    command.CommandText = @"INSERT INTO Coupon(
	                                         productname, description, amount)
	                                        VALUES ('IPhone X', 'IPhone Discount', 150);";
                    command.ExecuteNonQuery();

                    command.CommandText = @"INSERT INTO Coupon(
	                                         productname, description, amount)
	                                        VALUES ('Samsung 10', 'Samsung Discount', 250);";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migrated Posgresql DataBase");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An Error occured while migrationg the postgresql database");

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDataBase<TContext>(host, retryForAvailability);
                    }
                }
            }

            return host;
        }
    }
}
