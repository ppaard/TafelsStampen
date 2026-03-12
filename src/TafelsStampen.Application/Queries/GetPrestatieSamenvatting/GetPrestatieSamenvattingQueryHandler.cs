namespace TafelsStampen.Application.Queries.GetPrestatieSamenvatting;
using Microsoft.Extensions.Logging;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Exceptions;
using TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.ValueObjects;

public class GetPrestatieSamenvattingQueryHandler : IQueryHandler<GetPrestatieSamenvattingQuery, PrestatieSamenvattingDto>
{
    private readonly IGameSessionRepository _sessionRepository;
    private readonly IHallOfFameRepository _hallOfFameRepository;
    private readonly ILogger<GetPrestatieSamenvattingQueryHandler> _logger;

    public GetPrestatieSamenvattingQueryHandler(
        IGameSessionRepository sessionRepository,
        IHallOfFameRepository hallOfFameRepository,
        ILogger<GetPrestatieSamenvattingQueryHandler> logger)
    {
        _sessionRepository = sessionRepository;
        _hallOfFameRepository = hallOfFameRepository;
        _logger = logger;
    }

    public async Task<PrestatieSamenvattingDto> HandleAsync(GetPrestatieSamenvattingQuery query)
    {
        _logger.LogDebug("Prestatie samenvatting ophalen voor sessie {SessionId}", query.SessionId);

        var currentSession = await _sessionRepository.GetByIdAsync(query.SessionId)
            ?? throw new DomainException($"Sessie {query.SessionId} niet gevonden.");

        // Hall of Fame rang berekenen (alle entries voor deze tafel + modus)
        var alleEntries = await _hallOfFameRepository.GetAllAsync();
        var gefilterd = alleEntries
            .Where(e => e.TableNumber == query.TableNumber && e.Mode == query.Mode)
            .OrderBy(e => e.TotalTimeMs)
            .ToList();

        int rang = gefilterd.Count(e => e.TotalTimeMs < currentSession.TotalTimeMs) + 1;
        int aantalDeelnemers = gefilterd.Count;

        // Persoonlijke records (exclusief huidige sessie)
        var persoonlijkeEntries = gefilterd
            .Where(e => e.PlayerId == query.PlayerId && e.Id != currentSession.Id)
            .OrderBy(e => e.TotalTimeMs)
            .ToList();

        bool isEersteGame = persoonlijkeEntries.Count == 0;
        long? vorigeBesteMs = isEersteGame ? null : persoonlijkeEntries[0].TotalTimeMs;
        bool isNieuwRecord = !isEersteGame && currentSession.TotalTimeMs < persoonlijkeEntries[0].TotalTimeMs;

        // Verbeterde sommen
        var historischeSessies = await _sessionRepository.GetByPlayerTableModeAsync(query.PlayerId, query.TableNumber, query.Mode);
        var historisch = historischeSessies
            .Where(s => s.Id != query.SessionId)
            .ToList();

        var verbeterdeSommen = new List<SomVerbeteringDto>();

        if (historisch.Count > 0)
        {
            // Beste historische reactietijd per (multiplicand, multiplier)
            var historischeBeste = historisch
                .SelectMany(s => s.Answers)
                .GroupBy(a => (a.Multiplicand, a.Multiplier))
                .ToDictionary(g => g.Key, g => g.Min(a => a.ReactionTimeMs));

            foreach (var answer in currentSession.Answers)
            {
                var key = (answer.Multiplicand, answer.Multiplier);
                if (historischeBeste.TryGetValue(key, out var historiekeMs) && answer.ReactionTimeMs < historiekeMs)
                {
                    verbeterdeSommen.Add(new SomVerbeteringDto(answer.Multiplicand, answer.Multiplier, historiekeMs, answer.ReactionTimeMs));
                }
            }
        }

        return new PrestatieSamenvattingDto(
            isEersteGame,
            isNieuwRecord,
            currentSession.TotalTimeMs,
            vorigeBesteMs,
            rang,
            aantalDeelnemers,
            verbeterdeSommen);
    }
}
