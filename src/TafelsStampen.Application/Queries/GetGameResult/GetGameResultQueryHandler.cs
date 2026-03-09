namespace TafelsStampen.Application.Queries.GetGameResult;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Exceptions;
using TafelsStampen.Domain.Repositories;

public class GetGameResultQueryHandler : IQueryHandler<GetGameResultQuery, GameResultDto>
{
    private readonly IGameSessionRepository _sessionRepository;
    private readonly IPlayerRepository _playerRepository;

    public GetGameResultQueryHandler(IGameSessionRepository sessionRepository, IPlayerRepository playerRepository)
    {
        _sessionRepository = sessionRepository;
        _playerRepository = playerRepository;
    }

    public async Task<GameResultDto> HandleAsync(GetGameResultQuery query)
    {
        var session = await _sessionRepository.GetByIdAsync(query.SessionId)
            ?? throw new DomainException($"Sessie {query.SessionId} niet gevonden.");

        var player = await _playerRepository.GetByIdAsync(session.PlayerId)
            ?? throw new DomainException("Speler niet gevonden.");

        var answers = session.Answers
            .Select(a => new AnswerDto(a.Multiplicand, a.Multiplier, a.GivenAnswer, a.CorrectAnswer, a.IsCorrect, a.ReactionTimeMs))
            .ToList();

        return new GameResultDto(
            session.Id,
            player.Name.Value,
            session.TableNumber.Value,
            session.Mode.ToString(),
            session.TotalTimeMs,
            session.ErrorCount,
            answers);
    }
}
