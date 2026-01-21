using CQRS.Application.Events.Consumer;
using CQRS.Application.Events.Publisher;
using CQRS.Application.Handlers;
using CQRS.Infraestructure.Context;
using CQRS.Infraestructure.Context.MongoConfig;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<WriteContext>();
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<ReadContext>();
builder.Services.AddSingleton<IConnectionFactory>(builder => new ConnectionFactory { HostName = "localhost"});
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
