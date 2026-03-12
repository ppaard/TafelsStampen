namespace TafelsStampen.Application.Tests.Commands;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using TafelsStampen.Application.Commands.RegisterPlayer;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Repositories;

public class RegisterPlayerCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ValidName_SavesPlayerAndReturnsId()
    {
        var repo = new Mock<IPlayerRepository>();
        Player? saved = null;
        repo.Setup(r => r.SaveAsync(It.IsAny<Player>()))
            .Callback<Player>(p => saved = p)
            .Returns(Task.CompletedTask);

        var handler = new RegisterPlayerCommandHandler(repo.Object, NullLogger<RegisterPlayerCommandHandler>.Instance);
        var id = await handler.HandleAsync(new RegisterPlayerCommand("Piet"));

        saved.ShouldNotBeNull();
        saved!.Name.Value.ShouldBe("Piet");
        id.ShouldBe(saved.Id);
    }
}
