namespace TafelsStampen.Application.Tests.Commands;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using TafelsStampen.Application.Commands.StartGame;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.ValueObjects;

public class StartGameCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ValidCommand_SavesSessionAndReturnsId()
    {
        var repo = new Mock<IGameSessionRepository>();
        GameSession? saved = null;
        repo.Setup(r => r.SaveAsync(It.IsAny<GameSession>()))
            .Callback<GameSession>(s => saved = s)
            .Returns(Task.CompletedTask);

        var handler = new StartGameCommandHandler(repo.Object, NullLogger<StartGameCommandHandler>.Instance);
        var playerId = Guid.NewGuid();
        var id = await handler.HandleAsync(new StartGameCommand(playerId, 3, GameMode.Volgorde));

        saved.ShouldNotBeNull();
        saved!.PlayerId.ShouldBe(playerId);
        saved.TableNumber.Value.ShouldBe(3);
        id.ShouldBe(saved.Id);
    }
}
