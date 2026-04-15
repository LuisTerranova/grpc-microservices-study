using Microsoft.EntityFrameworkCore;
using sales.core.Extensions;
using stock.api.Data;
using stock.api.Data.Repositories;
using stock.api.Interfaces;
using stock.api.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Register gRPC server support
builder.Services.AddCustomGrpc();

// 2. With Aspire, the connection string name changes to the injected resource name ("stockdb")
builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("stockdb")
            ?? builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddOpenApi();

builder.Services.AddScoped<IStockItemRepository, StockItemRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 3. Map StockGrpcService so gRPC clients (like Sales API) can call it
app.MapGrpcService<StockGrpcService>();
app.MapGet("/", () => "Service is running! Use a gRPC client to call StockService.");

app.Run();
