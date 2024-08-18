using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.MassTransit;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System.Text;
using Test.API;
using Test.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()  // Write logs to the console
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "aspnetcore-logs-{0:yyyy.MM.dd}"
    })
    .CreateLogger();

// Set Serilog as the logging provider
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddSingleton<TokenService>();

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

builder.Services.AddAuthorization();

// Add authentication using JWT Bearer tokens
builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.RequireHttpsMetadata = false; // Disable HTTPS requirement
            options.SaveToken = true; // Save the token in HttpContext
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero, // No additional time allowed for token expiration
                ValidateIssuer = false, // Do not validate the token issuer
                ValidateAudience = false, // Do not validate the token audience
                ValidateLifetime = true, // Check if the token is expired
                ValidateIssuerSigningKey = true, // Ensure the token was signed with a valid key
                ValidIssuer = "me-core", // Specify the valid issuer
                ValidAudience = "me-core", // Specify the valid audience
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this-is-a-32-byte-long-key-12345678")) // Define the signing key
            };
        });

var app = builder.Build();

app.MapGet("/", () => "Hello World!")
    .RequireAuthorization(); // Require authorization to access this endpoint

// Configure the HTTP request pipeline.

app.MapCarter();

app.UseExceptionHandler(options => { });

// Enable authentication middleware
app.UseAuthentication();

// Enable authorization middleware
app.UseAuthorization();

app.Run();
