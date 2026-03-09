namespace TafelsStampen.Application.Queries.GetPlayers;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Repositories;

public class GetPlayersQueryHandler : IQueryHandler<GetPlayersQuery, IReadOnlyList<PlayerDto>>
{
    private readonly IPlayerRepository _playerRepository;

    public GetPlayersQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<IReadOnlyList<PlayerDto>> HandleAsync(GetPlayersQuery query)
    {
        var players = await _playerRepository.GetAllAsync();
        return players.Select(p => new PlayerDto(p.Id, p.Name.Value, p.CreatedAt)).ToList();
    }
}
