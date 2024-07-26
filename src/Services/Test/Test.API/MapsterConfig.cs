using BuildingBlocks.Messaging.Events;
using Mapster;
using Test.API.Models;

namespace Test.API;
public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<Product, ProductEvent>.NewConfig()
                .Map(dest => dest.ProductId, src => src.Id);
    }
}

