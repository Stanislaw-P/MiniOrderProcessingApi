using System.Xml.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using MiniOrderProcessingApi.DTO;
using MiniOrderProcessingApi.Interfaces;
using MiniOrderProcessingApi.Models;
using MiniOrderProcessingApi.Repositories;
using MiniOrderProcessingApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Singleton используем потому что в памяти приложения храним заказы
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddTransient<IDiscountStrategy, NoDiscountStrategy>();
builder.Services.AddTransient<IDiscountStrategy, PromoDiscountStrategy>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Mini Order Processing API");


app.MapGet("/orders/{id}", Results<NotFound, Ok<OrderDto>> (Guid id, IOrderRepository orderRepository) =>
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

app.MapPost("/orders/quote", (QuoteDto quote, IEnumerable<IDiscountStrategy> strategies) =>
{
    var strategy = strategies.FirstOrDefault(candidate => candidate.DiscountType == quote.DiscountType);
    if (strategy is null)
        return Results.BadRequest(new { message = "Unknown discount type." });

    var finalPrice = strategy.GetDiscountPrice(quote.Price);
    return Results.Ok(new { finalPrice });
})
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest);

app.Run();
