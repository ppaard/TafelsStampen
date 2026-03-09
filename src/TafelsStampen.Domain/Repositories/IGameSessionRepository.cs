namespace TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.Entities;

public interface IGameSessionRepository
{
    Task<GameSession?> GetByIdAsync(Guid id);
    Task SaveAsync(GameSession session);
}
