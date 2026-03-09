namespace TafelsStampen.Infrastructure.Tests.Repositories;
using Shouldly;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.ValueObjects;
using TafelsStampen.Infrastructure;
using TafelsStampen.Infrastructure.Repositories;

public class JsonGameSessionRepositoryTests : IDisposable
{
    private readonly string _tempDir;
    private readonly JsonGameSessionRepository _repo;

    public JsonGameSessionRepositoryTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        _repo = new JsonGameSessionRepository(new FilePathProvider(_tempDir));
    }

    [Fact]
    public async Task SaveAndGetById_ReturnsSession()
    {
        var session = new GameSession(Guid.NewGuid(), new TableNumber(3), GameMode.Volgorde);
        await _repo.SaveAsync(session);

        var result = await _repo.GetByIdAsync(session.Id);
        result.ShouldNotBeNull();
        result!.TableNumber.Value.ShouldBe(3);
        result.Mode.ShouldBe(GameMode.Volgorde);
    }

    [Fact]
    public async Task SaveAndGetById_WithAnswers_RestoresAnswers()
    {
        var session = new GameSession(Guid.NewGuid(), new TableNumber(5), GameMode.Willekeurig);
        session.AddAnswer(new Answer(5, 3, 15, 1200));
        session.AddAnswer(new Answer(5, 4, 20, 900));
        await _repo.SaveAsync(session);

        var result = await _repo.GetByIdAsync(session.Id);
        result.ShouldNotBeNull();
        result!.Answers.Count.ShouldBe(2);
        result.Answers[0].Multiplicand.ShouldBe(5);
        result.Answers[0].IsCorrect.ShouldBeTrue();
    }

    [Fact]
    public async Task GetById_NotFound_ReturnsNull()
    {
        var result = await _repo.GetByIdAsync(Guid.NewGuid());
        result.ShouldBeNull();
    }

    [Fact]
    public async Task Save_UpdatesExistingSession()
    {
        var playerId = Guid.NewGuid();
        var session = new GameSession(playerId, new TableNumber(3), GameMode.Volgorde);
        await _repo.SaveAsync(session);

        session.AddAnswer(new Answer(3, 3, 9, 500));
        session.Finish();
        await _repo.SaveAsync(session);

        var result = await _repo.GetByIdAsync(session.Id);
        result.ShouldNotBeNull();
        result!.IsFinished.ShouldBeTrue();
        result.Answers.Count.ShouldBe(1);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, true);
    }
}
