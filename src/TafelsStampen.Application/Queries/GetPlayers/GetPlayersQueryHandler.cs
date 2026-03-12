namespace TafelsStampen.Application.Queries.GetPlayers;
using Microsoft.Extensions.Logging;
using TafelsStampen.Application.DTOs;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Repositories;

public class GetPlayersQueryHandler : IQueryHandler<GetPlayersQuery, IReadOnlyList<PlayerDto>>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ILogger<GetPlayersQueryHandler> _logger;

    public GetPlayersQueryHandler(IPlayerRepository playerRepository, ILogger<GetPlayersQueryHandler> logger)
    {
        _playerRepository = playerRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<PlayerDto>> HandleAsync(GetPlayersQuery query)
    {
        _logger.LogDebug("Spelers ophalen");
        var players = await _playerRepository.GetAllAsync();
        var result = players.Select(p => new PlayerDto(p.Id, p.Name.Value, p.CreatedAt)).ToList();
        _logger.LogDebug("{Count} speler(s) opgehaald", result.Count);
        return result;
    }
}
