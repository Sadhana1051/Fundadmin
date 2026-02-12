using FundAdministration.Api.Entities;

namespace FundAdministration.Api.Repositories;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken ct = default);
    Task<IEnumerable<Transaction>> GetByInvestorAsync(Guid investorId, CancellationToken ct = default);
    Task<(decimal Subscribed, decimal Redeemed)> GetTotalsByFundIdAsync(Guid fundId, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}