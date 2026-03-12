namespace TafelsStampen.Application.Queries.GetPrestatieSamenvatting;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.ValueObjects;

public record GetPrestatieSamenvattingQuery(Guid SessionId, Guid PlayerId, int TableNumber, GameMode Mode) : IQuery<PrestatieSamenvattingDto>;
