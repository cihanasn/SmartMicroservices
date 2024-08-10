using BuildingBlocks.Behaviors;
using BuildingBlocks.Messaging.MassTransit;
using Carter;
using FluentValidation;
using Test.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

//Async Communication Services
builder.Services.AddMessageBroker(builder.Configuration);

builder.Services.RegisterMapsterConfiguration();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Configure the HTTP request pipeline.

app.MapCarter();

app.Run();
