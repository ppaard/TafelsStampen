namespace TafelsStampen.Application.Queries.GetHallOfFameOverall;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.ValueObjects;

public record GetHallOfFameOverallQuery(GameMode? ModeFilter = null, Guid? PlayerFilter = null) : IQuery<IReadOnlyList<HallOfFameEntryDto>>;
