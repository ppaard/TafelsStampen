using Shouldly;

namespace TafelsStampen.Domain.Tests.Entities;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.ValueObjects;

public class GameSessionTests
{
    private GameSession CreateSession() =>
        new(Guid.NewGuid(), new TableNumber(3), GameMode.Volgorde);

    [Fact]
    public void NewSession_IsNotFinished()
    {
        var session = CreateSession();
        session.IsFinished.ShouldBeFalse();
    }

    [Fact]
    public void AddAnswer_IncreasesAnswerCount()
    {
        var session = CreateSession();
        session.AddAnswer(new Answer(3, 1, 3, 1000));
        session.Answers.Count.ShouldBe(1);
    }

    [Fact]
    public void Finish_SetsIsFinished()
    {
        var session = CreateSession();
        session.Finish();
        session.IsFinished.ShouldBeTrue();
    }

    [Fact]
    public void AddAnswer_AfterFinish_Throws()
    {
        var session = CreateSession();
        session.Finish();
        Should.Throw<InvalidOperationException>(() => session.AddAnswer(new Answer(3, 1, 3, 500)));
    }

    [Fact]
    public void TotalTimeMs_SumsAllAnswers()
    {
        var session = CreateSession();
        session.AddAnswer(new Answer(3, 1, 3, 1000));
        session.AddAnswer(new Answer(3, 2, 6, 2000));
        session.TotalTimeMs.ShouldBe(3000);
    }

    [Fact]
    public void ErrorCount_CountsIncorrectAnswers()
    {
        var session = CreateSession();
        session.AddAnswer(new Answer(3, 1, 3, 1000)); // correct
        session.AddAnswer(new Answer(3, 2, 99, 1000)); // wrong
        session.ErrorCount.ShouldBe(1);
    }
}
