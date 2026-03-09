namespace TafelsStampen.Application.Queries.GetGameResult;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;

public record GetGameResultQuery(Guid SessionId) : IQuery<GameResultDto>;
