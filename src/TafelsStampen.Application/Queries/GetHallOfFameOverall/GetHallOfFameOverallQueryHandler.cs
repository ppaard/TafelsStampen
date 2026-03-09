namespace TafelsStampen.Application.Queries.GetHallOfFameOverall;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Repositories;

public class GetHallOfFameOverallQueryHandler : IQueryHandler<GetHallOfFameOverallQuery, IReadOnlyList<HallOfFameEntryDto>>
{
    private readonly IHallOfFameRepository _hallOfFameRepository;

    public GetHallOfFameOverallQueryHandler(IHallOfFameRepository hallOfFameRepository)
    {
        _hallOfFameRepository = hallOfFameRepository;
    }

    public async Task<IReadOnlyList<HallOfFameEntryDto>> HandleAsync(GetHallOfFameOverallQuery query)
    {
        var entries = await _hallOfFameRepository.GetAllAsync();
        return entries
            .OrderBy(e => e.TotalTimeMs)
            .ThenBy(e => e.ErrorCount)
            .Select((e, i) => new HallOfFameEntryDto(i + 1, e.PlayerName, e.TableNumber, e.TotalTimeMs, e.ErrorCount, e.Date))
            .ToList();
    }
}
