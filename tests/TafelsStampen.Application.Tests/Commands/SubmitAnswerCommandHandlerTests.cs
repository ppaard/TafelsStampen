namespace TafelsStampen.Application.Tests.Commands;
using Moq;
using Shouldly;
using TafelsStampen.Application.Commands.SubmitAnswer;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.ValueObjects;

public class SubmitAnswerCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_CorrectAnswer_ReturnsTrue()
    {
        var session = new GameSession(Guid.NewGuid(), new TableNumber(3), GameMode.Volgorde);
        var repo = new Mock<IGameSessionRepository>();
        repo.Setup(r => r.GetByIdAsync(session.Id)).ReturnsAsync(session);
        repo.Setup(r => r.SaveAsync(It.IsAny<GameSession>())).Returns(Task.CompletedTask);

        var handler = new SubmitAnswerCommandHandler(repo.Object);
        var result = await handler.HandleAsync(new SubmitAnswerCommand(session.Id, 3, 4, 12, 1500));

        result.ShouldBeTrue();
        session.Answers.Count.ShouldBe(1);
    }

    [Fact]
    public async Task HandleAsync_WrongAnswer_ReturnsFalse()
    {
        var session = new GameSession(Guid.NewGuid(), new TableNumber(3), GameMode.Volgorde);
        var repo = new Mock<IGameSessionRepository>();
        repo.Setup(r => r.GetByIdAsync(session.Id)).ReturnsAsync(session);
        repo.Setup(r => r.SaveAsync(It.IsAny<GameSession>())).Returns(Task.CompletedTask);

        var handler = new SubmitAnswerCommandHandler(repo.Object);
        var result = await handler.HandleAsync(new SubmitAnswerCommand(session.Id, 3, 4, 10, 1500));

        result.ShouldBeFalse();
    }
}
