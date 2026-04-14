using Microsoft.EntityFrameworkCore;
using stock.api.Data;
using stock.api.Interfaces;
using stock.api.Data.Repositories;
using stock.api.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Registra o suporte ao servidor gRPC!
builder.Services.AddGrpc();

// 2. Com o Aspire, o nome da string de conexão muda de "DefaultConnection" para a do recurso injetado ("stockdb")
builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("stockdb") ?? builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();

builder.Services.AddScoped<IItemRepository, ItemRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 3. Mapeia a rota do StockGrpcService para que clientes gRPC (como a Sales API) possam chamá-la
app.MapGrpcService<StockGrpcService>();
app.MapGet("/", () => "Service is running! Use a gRPC client to call StockService.");

app.Run();
