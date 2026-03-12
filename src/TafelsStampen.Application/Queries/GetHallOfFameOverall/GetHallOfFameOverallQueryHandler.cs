namespace TafelsStampen.Application.Queries.GetHallOfFameOverall;
using Microsoft.Extensions.Logging;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Repositories;

public class GetHallOfFameOverallQueryHandler : IQueryHandler<GetHallOfFameOverallQuery, IReadOnlyList<HallOfFameEntryDto>>
{
    private readonly IHallOfFameRepository _hallOfFameRepository;
    private readonly ILogger<GetHallOfFameOverallQueryHandler> _logger;

    public GetHallOfFameOverallQueryHandler(IHallOfFameRepository hallOfFameRepository, ILogger<GetHallOfFameOverallQueryHandler> logger)
    {
        _hallOfFameRepository = hallOfFameRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<HallOfFameEntryDto>> HandleAsync(GetHallOfFameOverallQuery query)
    {
        _logger.LogDebug("Algehele Hall of Fame ophalen");
        var entries = await _hallOfFameRepository.GetAllAsync();
        return entries
            .Where(e => query.ModeFilter == null || e.Mode == query.ModeFilter)
            .OrderBy(e => e.TotalTimeMs)
            .ThenBy(e => e.ErrorCount)
            .Select((e, i) => new HallOfFameEntryDto(i + 1, e.PlayerName, e.TableNumber, e.TotalTimeMs, e.ErrorCount, e.Date, e.Mode))
            .ToList();
    }
}
