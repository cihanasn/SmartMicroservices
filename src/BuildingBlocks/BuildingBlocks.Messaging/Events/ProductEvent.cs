namespace BuildingBlocks.Messaging.Events;

public record ProductEvent : IntegrationEvent
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
}

