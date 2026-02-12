using FundAdministration.Api.Entities;

namespace FundAdministration.Api.Repositories;

public interface IFundRepository
{
    Task<Fund?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Fund>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Fund fund, CancellationToken ct = default);
    void Update(Fund fund);
    void Remove(Fund fund);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
