using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
// DDD - Domain Driven Design
// CQRS
// Clean Architecture

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureService(builder.Configuration)
    .AddApiServices(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
