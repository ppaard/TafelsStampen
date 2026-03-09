namespace TafelsStampen.Application.Queries.GetHallOfFameByTable;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;

public record GetHallOfFameByTableQuery(int TableNumber) : IQuery<IReadOnlyList<HallOfFameEntryDto>>;
