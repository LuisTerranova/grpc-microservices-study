using Microsoft.EntityFrameworkCore;
using sales.api.Data;
using sales.api.Data.Repositories;
using sales.api.Interfaces;
using sales.api.Models;
using sales.contracts.Protos;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurando provedor de banco (usando "salesdb" injetado pelo AppHost do Aspire)
builder.Services.AddDbContext<SalesDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("salesdb")
            ?? builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// 2. G-RPC CLIENT INJECTOR! Aqui configuramos quem a Service vai chamar. Service Discovery Resolve!
builder.Services.AddGrpcClient<StockService.StockServiceClient>(options =>
{
    // "stock-api" é o nome dado lá no AppHost.Program.cs
    options.Address = new Uri("https+http://stock-api");
});

builder.Services.AddOpenApi();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 3. NOSSO ENDPOINT API REST PARA O SEU FRONTEND E NÃO GRPC
app.MapPost(
    "/api/orders",
    async (
        CreateOrderRequest request,
        IOrderRepository orderRepository,
        StockService.StockServiceClient stockClient
    ) =>
    {
        var newOrder = new Order
        {
            Id = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            Status = sales.api.Models.OrderStatus.Pending,
            TotalAmount = (decimal)request.Items.Sum(i => i.Price),
        };

        foreach (var item in request.Items)
        {
            // Supomos que "item.Name" carrega o GUID do produto, só para o exemplo didático.
            if (Guid.TryParse(item.Name, out var stockItemId))
            {
                var reserveRequest = new ReserveStockRequest
                {
                    Id = stockItemId.ToString(),
                    Quantity = 1,
                };

                try
                {
                    // AQUI A MÁGICA GRP-C ACONTECE DE FORMA SÍNCRONA
                    var stockResponse = await stockClient.ReserveStockAsync(reserveRequest);
                    if (!stockResponse.Success)
                        return Results.BadRequest(
                            new
                            {
                                Message = $"Falha ao reservar item {item.Name}: {stockResponse.Message}",
                            }
                        );
                }
                catch (Grpc.Core.RpcException ex)
                {
                    return Results.BadRequest(
                        new
                        {
                            Message = $"Estoque parece estar indisponível ou fora do ar: {ex.Status.Detail}",
                        }
                    );
                }
            }
        }

        await orderRepository.CreateAsync(newOrder);
        await orderRepository.SaveChangesAsync();

        // Num ambiente real, aqui invocaríamos o EVENTO (MassTransit) de "OrdemCriada" para o Payments API.

        return Results.Created($"/api/orders/{newOrder.Id}", newOrder);
    }
);

app.Run();
