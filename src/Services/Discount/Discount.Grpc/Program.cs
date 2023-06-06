using AutoMapper;
using Discount.Grpc.Extensions;
using Discount.Grpc.Mapper;
using Discount.Grpc.Repositories;
using Discount.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
var config = new MapperConfiguration(c =>
{
    c.AddProfile<DiscountProfile>();
});
builder.Services.AddSingleton<IMapper>(s => config.CreateMapper());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.MigrateDataBase<WebApplication>();
app.Run();
