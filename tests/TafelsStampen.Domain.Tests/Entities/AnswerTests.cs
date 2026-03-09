using Shouldly;

namespace TafelsStampen.Domain.Tests.Entities;
using TafelsStampen.Domain.Entities;

public class AnswerTests
{
    [Fact]
    public void Create_WithCorrectAnswer_IsCorrect()
    {
        var answer = new Answer(3, 4, 12, 1500);
        answer.IsCorrect.ShouldBeTrue();
        answer.CorrectAnswer.ShouldBe(12);
    }

    [Fact]
    public void Create_WithWrongAnswer_IsNotCorrect()
    {
        var answer = new Answer(3, 4, 10, 1500);
        answer.IsCorrect.ShouldBeFalse();
    }

    [Fact]
    public void ReactionTime_IsStored()
    {
        var answer = new Answer(2, 5, 10, 2000);
        answer.ReactionTimeMs.ShouldBe(2000);
    }
}
