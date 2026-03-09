namespace TafelsStampen.Application.Commands.FinishGame;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Exceptions;
using TafelsStampen.Domain.Repositories;

public class FinishGameCommandHandler : ICommandHandler<FinishGameCommand, Unit>
{
    private readonly IGameSessionRepository _sessionRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IHallOfFameRepository _hallOfFameRepository;

    public FinishGameCommandHandler(
        IGameSessionRepository sessionRepository,
        IPlayerRepository playerRepository,
        IHallOfFameRepository hallOfFameRepository)
    {
        _sessionRepository = sessionRepository;
        _playerRepository = playerRepository;
        _hallOfFameRepository = hallOfFameRepository;
    }

    public async Task<Unit> HandleAsync(FinishGameCommand command)
    {
        var session = await _sessionRepository.GetByIdAsync(command.SessionId)
            ?? throw new DomainException($"Sessie {command.SessionId} niet gevonden.");

        var player = await _playerRepository.GetByIdAsync(session.PlayerId)
            ?? throw new DomainException($"Speler niet gevonden.");

        session.Finish();
        await _sessionRepository.SaveAsync(session);

        var entry = new HallOfFameEntry(
            session.PlayerId,
            player.Name.Value,
            session.TableNumber.Value,
            session.TotalTimeMs,
            session.ErrorCount);
        await _hallOfFameRepository.SaveAsync(entry);

        return Unit.Value;
    }
}
