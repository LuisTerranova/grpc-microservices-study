using Aspire.Hosting;
using Aspire.Hosting.Postgres;
using Aspire.Hosting.RabbitMQ;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");

var salesDb = postgres.AddDatabase("salesdb");
var stockDb = postgres.AddDatabase("stockdb");
var paymentsDb = postgres.AddDatabase("paymentsdb");

var rabbitmq = builder.AddRabbitMQ("rabbitmq");

var stockApi = builder.AddProject<Projects.stock_api>("stock-api").WithReference(stockDb);

var salesApi = builder
    .AddProject<Projects.sales_api>("sales-api")
    .WithReference(salesDb)
    .WithReference(stockApi)
    .WithReference(rabbitmq);

var paymentsApi = builder
    .AddProject<Projects.payments_api>("payments-api")
    .WithReference(paymentsDb)
    .WithReference(rabbitmq);

builder.Build().Run();
