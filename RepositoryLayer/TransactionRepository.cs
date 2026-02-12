using FundAdministration.Api.Data;
using FundAdministration.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace FundAdministration.Api.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(Transaction transaction, CancellationToken ct = default) =>
        await _context.Transactions.AddAsync(transaction, ct);

    public async Task<IEnumerable<Transaction>> GetByInvestorAsync(Guid investorId, CancellationToken ct = default) =>
        await _context.Transactions.Where(t => t.InvestorId == investorId).ToListAsync(ct);

    public async Task<(decimal Subscribed, decimal Redeemed)> GetTotalsByFundIdAsync(Guid fundId, CancellationToken ct = default)
    {
        var transactions = await _context.Investors
            .Where(i => i.FundId == fundId)
            .SelectMany(i => i.Transactions)
            .ToListAsync(ct);

        var subscribed = transactions.Where(t => t.Type == TransactionType.Subscription).Sum(t => t.Amount);
        var redeemed = transactions.Where(t => t.Type == TransactionType.Redemption).Sum(t => t.Amount);
        return (subscribed, redeemed);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _context.SaveChangesAsync(ct);
}