using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class ProducEventHandler
    (ISender sender, ILogger<ProductEvent> logger) : IConsumer<ProductEvent>
{
    public async Task Consume(ConsumeContext<ProductEvent> context)
    {
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

    }
}
