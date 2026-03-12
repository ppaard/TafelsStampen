namespace TafelsStampen.Application.Queries.GetHallOfFameByTable;
using Microsoft.Extensions.Logging;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Repositories;

public class GetHallOfFameByTableQueryHandler : IQueryHandler<GetHallOfFameByTableQuery, IReadOnlyList<HallOfFameEntryDto>>
{
    private readonly IHallOfFameRepository _hallOfFameRepository;
    private readonly ILogger<GetHallOfFameByTableQueryHandler> _logger;

    public GetHallOfFameByTableQueryHandler(IHallOfFameRepository hallOfFameRepository, ILogger<GetHallOfFameByTableQueryHandler> logger)
    {
        _hallOfFameRepository = hallOfFameRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<HallOfFameEntryDto>> HandleAsync(GetHallOfFameByTableQuery query)
    {
        _logger.LogDebug("Hall of Fame ophalen voor tafel {TableNumber}", query.TableNumber);
        var entries = await _hallOfFameRepository.GetByTableAsync(query.TableNumber);
        return entries
            .Where(e => query.ModeFilter == null || e.Mode == query.ModeFilter)
            .Where(e => query.PlayerFilter == null || e.PlayerId == query.PlayerFilter)
            .OrderBy(e => e.TotalTimeMs)
            .ThenBy(e => e.ErrorCount)
            .Select((e, i) => new HallOfFameEntryDto(i + 1, e.PlayerName, e.TableNumber, e.TotalTimeMs, e.ErrorCount, e.Date, e.Mode))
            .ToList();
    }
}
