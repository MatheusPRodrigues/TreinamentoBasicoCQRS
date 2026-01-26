using CQRS.Application.Events.Consumer;
using CQRS.Application.Events.Publisher;
using CQRS.Application.Handlers;
using CQRS.Infraestructure.Context;
using CQRS.Infraestructure.Context.MongoConfig;
using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configuração tipada para objetos do appsettings.json
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<RabbitConfig>(builder.Configuration.GetSection("RabbitMQ"));

// Instância dos factory
builder.Services.AddSingleton<IAbstractFactory<SqlConnection>, SqlContext>();
builder.Services.AddSingleton<IAbstractFactory<IMongoDatabase>, MongoContext>();
builder.Services.AddSingleton<IAbstractFactory<ConnectionFactory>, RabbitContext>();

builder.Services.AddHostedService<ConsumeQueue>();
builder.Services.AddSingleton<PublishMessage>();
builder.Services.AddSingleton<CreateProductHandler>();
builder.Services.AddSingleton<GetProductByIdHandler>();
builder.Services.AddSingleton<GetAllProductsHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
