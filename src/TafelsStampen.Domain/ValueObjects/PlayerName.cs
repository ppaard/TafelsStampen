namespace TafelsStampen.Domain.ValueObjects;
using TafelsStampen.Domain.Exceptions;

public sealed record PlayerName
{
    public string Value { get; }

    public PlayerName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Spelernaam mag niet leeg zijn.");
        if (value.Trim().Length > 30)
            throw new DomainException("Spelernaam mag maximaal 30 tekens bevatten.");
        Value = value.Trim();
    }

    public override string ToString() => Value;
    public static implicit operator string(PlayerName name) => name.Value;
}
