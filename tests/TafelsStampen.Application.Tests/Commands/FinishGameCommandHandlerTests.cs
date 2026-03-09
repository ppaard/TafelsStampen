namespace TafelsStampen.Application.Tests.Commands;
using Moq;
using Shouldly;
using TafelsStampen.Application.Commands.FinishGame;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Exceptions;
using TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.ValueObjects;

public class FinishGameCommandHandlerTests
{
    private static (GameSession session, Player player) CreateTestData()
    {
        var player = Player.Create("Jan");
        var session = new GameSession(player.Id, new TableNumber(3), GameMode.Volgorde);
        session.AddAnswer(new Answer(3, 1, 3, 1000));
        session.AddAnswer(new Answer(3, 2, 99, 2000)); // fout
        return (session, player);
    }

    [Fact]
    public async Task HandleAsync_FinishesSessionAndSavesHallOfFameEntry()
    {
        var (session, player) = CreateTestData();

        var sessionRepo = new Mock<IGameSessionRepository>();
        sessionRepo.Setup(r => r.GetByIdAsync(session.Id)).ReturnsAsync(session);
        sessionRepo.Setup(r => r.SaveAsync(It.IsAny<GameSession>())).Returns(Task.CompletedTask);

        var playerRepo = new Mock<IPlayerRepository>();
        playerRepo.Setup(r => r.GetByIdAsync(player.Id)).ReturnsAsync(player);

        HallOfFameEntry? savedEntry = null;
        var hofRepo = new Mock<IHallOfFameRepository>();
        hofRepo.Setup(r => r.SaveAsync(It.IsAny<HallOfFameEntry>()))
            .Callback<HallOfFameEntry>(e => savedEntry = e)
            .Returns(Task.CompletedTask);

        var handler = new FinishGameCommandHandler(sessionRepo.Object, playerRepo.Object, hofRepo.Object);
        await handler.HandleAsync(new FinishGameCommand(session.Id));

        session.IsFinished.ShouldBeTrue();
        savedEntry.ShouldNotBeNull();
        savedEntry!.PlayerName.ShouldBe("Jan");
        savedEntry.TableNumber.ShouldBe(3);
        savedEntry.ErrorCount.ShouldBe(1);
        savedEntry.TotalTimeMs.ShouldBe(3000);
    }

    [Fact]
    public async Task HandleAsync_SessionNotFound_ThrowsDomainException()
    {
        var sessionRepo = new Mock<IGameSessionRepository>();
        sessionRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((GameSession?)null);

        var handler = new FinishGameCommandHandler(
            sessionRepo.Object,
            new Mock<IPlayerRepository>().Object,
            new Mock<IHallOfFameRepository>().Object);

        await Should.ThrowAsync<DomainException>(() =>
            handler.HandleAsync(new FinishGameCommand(Guid.NewGuid())));
    }
}
