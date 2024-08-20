using MediatR;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        var order = domainEvent.order;

        Console.WriteLine($"Order Created with ID: {order.Id} at {order.CreatedAt}");

        return Task.CompletedTask;
    }
}
