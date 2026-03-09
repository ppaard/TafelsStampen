namespace TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.Entities;

public interface IHallOfFameRepository
{
    Task<IReadOnlyList<HallOfFameEntry>> GetByTableAsync(int tableNumber);
    Task<IReadOnlyList<HallOfFameEntry>> GetAllAsync();
    Task SaveAsync(HallOfFameEntry entry);
}
