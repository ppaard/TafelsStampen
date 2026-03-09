namespace TafelsStampen.Domain.Entities;

public class Answer
{
    public int Multiplicand { get; private set; }  // a in a×b
    public int Multiplier { get; private set; }    // b in a×b
    public int GivenAnswer { get; private set; }
    public bool IsCorrect { get; private set; }
    public long ReactionTimeMs { get; private set; }

    public int CorrectAnswer => Multiplicand * Multiplier;

    private Answer() { }

    public Answer(int multiplicand, int multiplier, int givenAnswer, long reactionTimeMs)
    {
        Multiplicand = multiplicand;
        Multiplier = multiplier;
        GivenAnswer = givenAnswer;
        IsCorrect = givenAnswer == multiplicand * multiplier;
        ReactionTimeMs = reactionTimeMs;
    }

    public static Answer Reconstitute(int multiplicand, int multiplier, int givenAnswer, bool isCorrect, long reactionTimeMs) =>
        new() { Multiplicand = multiplicand, Multiplier = multiplier, GivenAnswer = givenAnswer, IsCorrect = isCorrect, ReactionTimeMs = reactionTimeMs };
}
