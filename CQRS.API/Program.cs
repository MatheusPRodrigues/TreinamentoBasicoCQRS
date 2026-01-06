using CQRS.Application.Handlers;
using CQRS.Infraestructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<CreateProductHandler>();
builder.Services.AddSingleton<GetProductByIdHandler>();
builder.Services.AddSingleton<GetAllProductsHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
