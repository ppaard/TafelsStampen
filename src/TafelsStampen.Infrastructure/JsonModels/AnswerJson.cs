namespace TafelsStampen.Infrastructure.JsonModels;

public class AnswerJson
{
    public int Multiplicand { get; set; }
    public int Multiplier { get; set; }
    public int GivenAnswer { get; set; }
    public bool IsCorrect { get; set; }
    public long ReactionTimeMs { get; set; }
}
