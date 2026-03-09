namespace TafelsStampen.Domain.ValueObjects;
using TafelsStampen.Domain.Exceptions;

public sealed record TableNumber
{
    public int Value { get; }

    public TableNumber(int value)
    {
        if (value < 1 || value > 10)
            throw new DomainException("Tafelnummer moet tussen 1 en 10 liggen.");
        Value = value;
    }

    public override string ToString() => Value.ToString();
    public static implicit operator int(TableNumber t) => t.Value;
}
