namespace TafelsStampen.Application.Queries.GetHallOfFameOverall;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;

public record GetHallOfFameOverallQuery : IQuery<IReadOnlyList<HallOfFameEntryDto>>;
