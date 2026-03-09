namespace TafelsStampen.Application.Queries.GetPlayers;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;

public record GetPlayersQuery : IQuery<IReadOnlyList<PlayerDto>>;
