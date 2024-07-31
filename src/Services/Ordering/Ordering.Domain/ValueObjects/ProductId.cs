namespace Ordering.Domain.ValueObjects;
public record ProductId
{
    public Guid Value { get; set; }

    private ProductId(Guid value) => Value = value;

    public static ProductId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new Exception("ProductId cannot be empty.");
        }

        return new ProductId(value);
    }
}

