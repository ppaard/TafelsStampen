namespace TafelsStampen.Application.Queries.GetHallOfFameByTable;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Application.Queries;
using TafelsStampen.Domain.ValueObjects;

public record GetHallOfFameByTableQuery(
    int TableNumber,
    GameMode? ModeFilter = null,
    Guid? PlayerFilter = null,
    HallOfFamePeriode PeriodeFilter = HallOfFamePeriode.DezeWeek)
    : IQuery<IReadOnlyList<HallOfFameEntryDto>>;
