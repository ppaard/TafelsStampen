namespace TafelsStampen.Application.Queries.GetHallOfFameByTable;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Repositories;

public class GetHallOfFameByTableQueryHandler : IQueryHandler<GetHallOfFameByTableQuery, IReadOnlyList<HallOfFameEntryDto>>
{
    private readonly IHallOfFameRepository _hallOfFameRepository;

    public GetHallOfFameByTableQueryHandler(IHallOfFameRepository hallOfFameRepository)
    {
        _hallOfFameRepository = hallOfFameRepository;
    }

    public async Task<IReadOnlyList<HallOfFameEntryDto>> HandleAsync(GetHallOfFameByTableQuery query)
    {
        var entries = await _hallOfFameRepository.GetByTableAsync(query.TableNumber);
        return entries
            .OrderBy(e => e.TotalTimeMs)
            .ThenBy(e => e.ErrorCount)
            .Select((e, i) => new HallOfFameEntryDto(i + 1, e.PlayerName, e.TableNumber, e.TotalTimeMs, e.ErrorCount, e.Date))
            .ToList();
    }
}
