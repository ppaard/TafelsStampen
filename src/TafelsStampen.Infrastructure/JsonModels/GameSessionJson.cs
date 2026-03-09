namespace TafelsStampen.Infrastructure.JsonModels;

public class GameSessionJson
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public int TableNumber { get; set; }
    public int Mode { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public List<AnswerJson> Answers { get; set; } = new();
}
