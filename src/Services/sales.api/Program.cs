using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using sales.api.Data;
using sales.api.Data.Repositories;
using sales.api.Interfaces;
using sales.api.Models;
using sales.api.Services;
using sales.contracts.Protos;
using sales.contracts.Requests;
using sales.Contracts.Responses;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Configuration (using "salesdb" injected by Aspire AppHost)
builder.Services.AddDbContext<SalesDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("salesdb")
            ?? builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// 2. gRPC Client Registration. Service Discovery resolves the target automatically!
builder.Services.AddGrpcClient<StockService.StockServiceClient>(options =>
{
    // "stock-api" is the name defined in AppHost.cs
    options.Address = new Uri("https+http://stock-api");
});

builder.Services.AddOpenApi();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<SalesService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost(
    "/api/orders",
    async (CreateOrderRequest request, SalesService salesService) =>
    {
        var result = await salesService.CreateOrder(request);

        if (result.IsSuccess)
            return Results.Created($"/api/orders/{result.Data?.Id}", result);

        return Results.Json(result, statusCode: result.StatusCode);
    }
);

app.Run();
