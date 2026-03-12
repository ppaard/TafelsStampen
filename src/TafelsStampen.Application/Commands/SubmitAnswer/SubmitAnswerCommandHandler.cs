namespace TafelsStampen.Application.Commands.SubmitAnswer;
using Microsoft.Extensions.Logging;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Exceptions;
using TafelsStampen.Domain.Repositories;

public class SubmitAnswerCommandHandler : ICommandHandler<SubmitAnswerCommand, bool>
{
    private readonly IGameSessionRepository _sessionRepository;
    private readonly ILogger<SubmitAnswerCommandHandler> _logger;

    public SubmitAnswerCommandHandler(IGameSessionRepository sessionRepository, ILogger<SubmitAnswerCommandHandler> logger)
    {
        _sessionRepository = sessionRepository;
        _logger = logger;
    }

    public async Task<bool> HandleAsync(SubmitAnswerCommand command)
    {
        _logger.LogDebug("Antwoord indienen voor sessie {SessionId}: {Multiplicand} x {Multiplier} = {GivenAnswer}",
            command.SessionId, command.Multiplicand, command.Multiplier, command.GivenAnswer);

        var session = await _sessionRepository.GetByIdAsync(command.SessionId)
            ?? throw new DomainException($"Sessie {command.SessionId} niet gevonden.");

        var answer = new Answer(command.Multiplicand, command.Multiplier, command.GivenAnswer, command.ReactionTimeMs);
        session.AddAnswer(answer);
        await _sessionRepository.SaveAsync(session);

        _logger.LogDebug("Antwoord voor sessie {SessionId} is {IsCorrect}", command.SessionId, answer.IsCorrect ? "correct" : "fout");
        return answer.IsCorrect;
    }
}
