namespace TafelsStampen.Infrastructure.Repositories;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.ValueObjects;
using TafelsStampen.Infrastructure.JsonModels;

public class JsonGameSessionRepository : JsonRepositoryBase<GameSessionJson>, IGameSessionRepository
{
    public JsonGameSessionRepository(FilePathProvider pathProvider)
        : base(pathProvider.SessionsFile) { }

    public async Task<GameSession?> GetByIdAsync(Guid id)
    {
        var all = await ReadAllAsync();
        var match = all.FirstOrDefault(s => s.Id == id);
        return match is null ? null : MapToDomain(match);
    }

    public async Task SaveAsync(GameSession session)
    {
        var all = await ReadAllAsync();
        var idx = all.FindIndex(s => s.Id == session.Id);
        var json = MapToJson(session);
        if (idx >= 0) all[idx] = json;
        else all.Add(json);
        await WriteAllAsync(all);
    }

    private static GameSession MapToDomain(GameSessionJson j)
    {
        var answers = j.Answers.Select(a =>
            Answer.Reconstitute(a.Multiplicand, a.Multiplier, a.GivenAnswer, a.IsCorrect, a.ReactionTimeMs));
        return GameSession.Reconstitute(j.Id, j.PlayerId, new TableNumber(j.TableNumber),
            (GameMode)j.Mode, j.StartedAt, j.FinishedAt, answers);
    }

    private static GameSessionJson MapToJson(GameSession s) => new()
    {
        Id = s.Id,
        PlayerId = s.PlayerId,
        TableNumber = s.TableNumber.Value,
        Mode = (int)s.Mode,
        StartedAt = s.StartedAt,
        FinishedAt = s.FinishedAt,
        Answers = s.Answers.Select(a => new AnswerJson
        {
            Multiplicand = a.Multiplicand,
            Multiplier = a.Multiplier,
            GivenAnswer = a.GivenAnswer,
            IsCorrect = a.IsCorrect,
            ReactionTimeMs = a.ReactionTimeMs
        }).ToList()
    };
}
