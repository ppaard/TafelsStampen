namespace TafelsStampen.Application.Tests.Queries;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using TafelsStampen.Application.Queries.GetGameResult;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Exceptions;
using TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.ValueObjects;

public class GetGameResultQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsCorrectGameResultDto()
    {
        var player = Player.Create("Jan");
        var session = new GameSession(player.Id, new TableNumber(4), GameMode.Willekeurig);
        session.AddAnswer(new Answer(4, 1, 4, 1200));   // correct
        session.AddAnswer(new Answer(4, 2, 99, 800));   // fout

        var sessionRepo = new Mock<IGameSessionRepository>();
        sessionRepo.Setup(r => r.GetByIdAsync(session.Id)).ReturnsAsync(session);

        var playerRepo = new Mock<IPlayerRepository>();
        playerRepo.Setup(r => r.GetByIdAsync(player.Id)).ReturnsAsync(player);

        var handler = new GetGameResultQueryHandler(sessionRepo.Object, playerRepo.Object, NullLogger<GetGameResultQueryHandler>.Instance);
        var result = await handler.HandleAsync(new GetGameResultQuery(session.Id));

        result.PlayerName.ShouldBe("Jan");
        result.TableNumber.ShouldBe(4);
        result.Mode.ShouldBe("Willekeurig");
        result.TotalTimeMs.ShouldBe(2000);
        result.ErrorCount.ShouldBe(1);
        result.Answers.Count.ShouldBe(2);
        result.Answers[0].IsCorrect.ShouldBeTrue();
        result.Answers[1].IsCorrect.ShouldBeFalse();
        result.Answers[1].CorrectAnswer.ShouldBe(8);
    }

    [Fact]
    public async Task HandleAsync_SessionNotFound_ThrowsDomainException()
    {
        var sessionRepo = new Mock<IGameSessionRepository>();
        sessionRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((GameSession?)null);

        var handler = new GetGameResultQueryHandler(sessionRepo.Object, new Mock<IPlayerRepository>().Object, NullLogger<GetGameResultQueryHandler>.Instance);

        await Should.ThrowAsync<DomainException>(() =>
            handler.HandleAsync(new GetGameResultQuery(Guid.NewGuid())));
    }
}
