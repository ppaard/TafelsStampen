namespace TafelsStampen.Application.Commands.SubmitAnswer;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Exceptions;
using TafelsStampen.Domain.Repositories;

public class SubmitAnswerCommandHandler : ICommandHandler<SubmitAnswerCommand, bool>
{
    private readonly IGameSessionRepository _sessionRepository;

    public SubmitAnswerCommandHandler(IGameSessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<bool> HandleAsync(SubmitAnswerCommand command)
    {
        var session = await _sessionRepository.GetByIdAsync(command.SessionId)
            ?? throw new DomainException($"Sessie {command.SessionId} niet gevonden.");

        var answer = new Answer(command.Multiplicand, command.Multiplier, command.GivenAnswer, command.ReactionTimeMs);
        session.AddAnswer(answer);
        await _sessionRepository.SaveAsync(session);
        return answer.IsCorrect;
    }
}
