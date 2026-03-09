namespace TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.ValueObjects;

public class GameSession
{
    public Guid Id { get; private set; }
    public Guid PlayerId { get; private set; }
    public TableNumber TableNumber { get; private set; }
    public GameMode Mode { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }

    private readonly List<Answer> _answers = new();
    public IReadOnlyList<Answer> Answers => _answers.AsReadOnly();

    public bool IsFinished => FinishedAt.HasValue;
    public long TotalTimeMs => _answers.Sum(a => a.ReactionTimeMs);
    public int ErrorCount => _answers.Count(a => !a.IsCorrect);

    private GameSession() { }

    public GameSession(Guid playerId, TableNumber tableNumber, GameMode mode)
    {
        Id = Guid.NewGuid();
        PlayerId = playerId;
        TableNumber = tableNumber;
        Mode = mode;
        StartedAt = DateTime.UtcNow;
    }

    public void AddAnswer(Answer answer)
    {
        if (IsFinished)
            throw new InvalidOperationException("Sessie is al afgerond.");
        _answers.Add(answer);
    }

    public void Finish()
    {
        if (IsFinished)
            throw new InvalidOperationException("Sessie is al afgerond.");
        FinishedAt = DateTime.UtcNow;
    }

    public static GameSession Reconstitute(Guid id, Guid playerId, TableNumber tableNumber, GameMode mode, DateTime startedAt, DateTime? finishedAt, IEnumerable<Answer> answers)
    {
        var session = new GameSession
        {
            Id = id,
            PlayerId = playerId,
            TableNumber = tableNumber,
            Mode = mode,
            StartedAt = startedAt,
            FinishedAt = finishedAt
        };
        foreach (var answer in answers)
            session._answers.Add(answer);
        return session;
    }
}
