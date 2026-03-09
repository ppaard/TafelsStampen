namespace TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.ValueObjects;

public class Player
{
    public Guid Id { get; private set; }
    public PlayerName Name { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Player() { } // for serialization

    public Player(PlayerName name)
    {
        Id = Guid.NewGuid();
        Name = name;
        CreatedAt = DateTime.UtcNow;
    }

    public static Player Create(string name) => new(new PlayerName(name));

    public static Player Reconstitute(Guid id, PlayerName name, DateTime createdAt) =>
        new() { Id = id, Name = name, CreatedAt = createdAt };
}
