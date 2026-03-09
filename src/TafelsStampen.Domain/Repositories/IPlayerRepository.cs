namespace TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.Entities;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Player>> GetAllAsync();
    Task SaveAsync(Player player);
}
