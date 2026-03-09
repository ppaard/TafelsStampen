namespace TafelsStampen.Infrastructure.JsonModels;

public class PlayerJson
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
