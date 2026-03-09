namespace TafelsStampen.Application.Commands.RegisterPlayer;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Repositories;

public class RegisterPlayerCommandHandler : ICommandHandler<RegisterPlayerCommand, Guid>
{
    private readonly IPlayerRepository _playerRepository;

    public RegisterPlayerCommandHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<Guid> HandleAsync(RegisterPlayerCommand command)
    {
        var player = Player.Create(command.Name);
        await _playerRepository.SaveAsync(player);
        return player.Id;
    }
}
