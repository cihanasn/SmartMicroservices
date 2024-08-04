using BuildingBlocks.Pagination;
using MediatR;
using Ordering.API;
using Ordering.Application;
using Ordering.Application.Orders.Queries.GetOrders;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;
// DDD - Domain Driven Design
// CQRS
// Clean Architecture

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureService(builder.Configuration)
    .AddApiServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.MapGet("/", () => "Hello World!");

app.MapGet("/test", async ([AsParameters] PaginationRequest request, ISender sender) => 
{
    var result = await sender.Send(new GetOrdersQuery(request));

    return Results.Ok("OK");
});

app.Run();
