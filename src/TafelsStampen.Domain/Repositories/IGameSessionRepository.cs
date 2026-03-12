namespace TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.ValueObjects;

public interface IGameSessionRepository
{
    Task<GameSession?> GetByIdAsync(Guid id);
    Task SaveAsync(GameSession session);
    Task<IReadOnlyList<GameSession>> GetByPlayerTableModeAsync(Guid playerId, int tableNumber, GameMode mode);
}
