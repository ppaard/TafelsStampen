namespace TafelsStampen.Application.Queries.GetHallOfFameOverall;
using Microsoft.Extensions.Logging;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Application.Queries;
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
        var today = DateTime.UtcNow.Date;
        DateTime? vanafDatum = query.PeriodeFilter switch
        {
            HallOfFamePeriode.Vandaag   => today,
            HallOfFamePeriode.DezeWeek  => today.AddDays(-(((int)today.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7)),
            HallOfFamePeriode.DezeMaand => new DateTime(today.Year, today.Month, 1),
            HallOfFamePeriode.DitJaar   => new DateTime(today.Year, 1, 1),
            _                           => null
        };
        return entries
            .Where(e => vanafDatum == null || e.Date.Date >= vanafDatum)
            .Where(e => query.ModeFilter == null || e.Mode == query.ModeFilter)
            .Where(e => query.PlayerFilter == null || e.PlayerId == query.PlayerFilter)
            .OrderBy(e => e.TotalTimeMs)
            .ThenBy(e => e.ErrorCount)
            .Select((e, i) => new HallOfFameEntryDto(i + 1, e.PlayerName, e.TableNumber, e.TotalTimeMs, e.ErrorCount, e.Date, e.Mode))
            .ToList();
    }
}
