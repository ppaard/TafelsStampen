namespace TafelsStampen.Application.Commands.RegisterPlayer;
using Microsoft.Extensions.Logging;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Repositories;

public class RegisterPlayerCommandHandler : ICommandHandler<RegisterPlayerCommand, Guid>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ILogger<RegisterPlayerCommandHandler> _logger;

    public RegisterPlayerCommandHandler(IPlayerRepository playerRepository, ILogger<RegisterPlayerCommandHandler> logger)
    {
        _playerRepository = playerRepository;
        _logger = logger;
    }

    public async Task<Guid> HandleAsync(RegisterPlayerCommand command)
    {
        _logger.LogDebug("Speler registreren met naam {Name}", command.Name);
        var player = Player.Create(command.Name);
        await _playerRepository.SaveAsync(player);
        _logger.LogInformation("Speler geregistreerd: {PlayerId} ({Name})", player.Id, command.Name);
        return player.Id;
    }
}
