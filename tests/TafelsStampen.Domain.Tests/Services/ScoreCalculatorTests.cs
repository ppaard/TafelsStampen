namespace TafelsStampen.Domain.Tests.Services;
using Shouldly;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Services;
using TafelsStampen.Domain.ValueObjects;

public class ScoreCalculatorTests
{
    private static GameSession CreateSessionWithAnswers(params (int b, int gegeven, long ms)[] antwoorden)
    {
        var session = new GameSession(Guid.NewGuid(), new TableNumber(3), GameMode.Volgorde);
        foreach (var (b, gegeven, ms) in antwoorden)
            session.AddAnswer(new Answer(3, b, gegeven, ms));
        return session;
    }

    [Fact]
    public void CalculateTotalTime_SumsAllReactionTimes()
    {
        var session = CreateSessionWithAnswers((1, 3, 1000), (2, 6, 2000), (3, 9, 500));

        ScoreCalculator.CalculateTotalTime(session).ShouldBe(3500);
    }

    [Fact]
    public void CalculateErrors_CountsWrongAnswers()
    {
        var session = CreateSessionWithAnswers((1, 3, 1000), (2, 99, 1000), (3, 99, 1000));

        ScoreCalculator.CalculateErrors(session).ShouldBe(2);
    }

    [Fact]
    public void CalculateRank_FastestIsFirst()
    {
        var e1 = new HallOfFameEntry(Guid.NewGuid(), "Jan",  3, 30000, 0);
        var e2 = new HallOfFameEntry(Guid.NewGuid(), "Kees", 3, 50000, 0);
        var all = new[] { e1, e2 };

        ScoreCalculator.CalculateRank(e1, all).ShouldBe(1);
        ScoreCalculator.CalculateRank(e2, all).ShouldBe(2);
    }

    [Fact]
    public void CalculateRank_TiesBrokenByFewerErrors()
    {
        var e1 = new HallOfFameEntry(Guid.NewGuid(), "Jan",  3, 30000, 1);
        var e2 = new HallOfFameEntry(Guid.NewGuid(), "Kees", 3, 30000, 0);
        var all = new[] { e1, e2 };

        ScoreCalculator.CalculateRank(e2, all).ShouldBe(1);
        ScoreCalculator.CalculateRank(e1, all).ShouldBe(2);
    }

    [Fact]
    public void CalculateRank_SingleEntry_IsRankOne()
    {
        var entry = new HallOfFameEntry(Guid.NewGuid(), "Jan", 3, 30000, 0);

        ScoreCalculator.CalculateRank(entry, new[] { entry }).ShouldBe(1);
    }
}
