using FundAdministration.Api.Entities;

namespace FundAdministration.Api.Repositories;

public interface IInvestorRepository
{
    Task<Investor?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Investor>> GetByFundIdAsync(Guid fundId, CancellationToken ct = default);
    Task<IEnumerable<Investor>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Investor investor, CancellationToken ct = default);
    void Update(Investor investor);
    void Remove(Investor investor);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
