using System.Xml.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using MiniOrderProcessingApi.DTO;
using MiniOrderProcessingApi.Interfaces;
using MiniOrderProcessingApi.Models;
using MiniOrderProcessingApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Singleton используем потому что в памяти приложения храним заказы
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Mini Order Processing API");


app.MapGet("/orders/{id}", Results<NotFound, Ok<OrderDto>>  (Guid id, IOrderRepository orderRepository) =>
{
    var existingOrder = orderRepository.GetById(id);
    if (existingOrder == null)
        return TypedResults.NotFound();

    var orderResult = new OrderDto
    {
        Id = existingOrder.Id,
        ProductName = existingOrder.ProductName
    };

    return TypedResults.Ok(orderResult);
})
.WithName("GetOrderById");

app.MapPost("/orders", (CreateOrderDto order, IOrderRepository orderRepository) =>
{
    var newOrder = new Order
    {
        Id = Guid.NewGuid(),
        ProductName = order.ProductName
    };

    orderRepository.Add(newOrder);


    var orderResult = new OrderDto
    {
        Id = newOrder.Id,
        ProductName = newOrder.ProductName
    };

    return TypedResults.CreatedAtRoute(
        orderResult,
        routeName: "GetOrderById",
        routeValues: new { Id = orderResult.Id }
    );
});

app.Run();
